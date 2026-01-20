import { ChangeDetectionStrategy, Component, OnInit, computed, signal } from '@angular/core';
import { PurchaseOrderService } from '../../services/purchase-order';
import { PurchaseOrder, PurchaseOrderQuery, CreatePurchaseOrderDto, UpdatePurchaseOrderDto } from '../../models/purchase-order.model';
import { PurchaseOrderFormComponent } from '../purchase-order-form/purchase-order-form';

@Component({
  selector: 'app-purchase-order-list',
  templateUrl: './purchase-order-list.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [PurchaseOrderFormComponent],
})
export class PurchaseOrderListComponent implements OnInit {
  protected readonly purchaseOrders = signal<PurchaseOrder[]>([]);
  protected readonly totalRecords = signal(0);
  protected readonly page = signal(1);
  protected readonly pageSize = signal(10);
  protected readonly supplier = signal('');
  protected readonly status = signal('');
  protected readonly startDate = signal('');
  protected readonly endDate = signal('');
  protected readonly sortBy = signal<'OrderDate' | 'SupplierName' | 'TotalAmount' | 'PoNumber'>(
    'OrderDate',
  );
  protected readonly sortDir = signal<'asc' | 'desc'>('asc');
  protected readonly loading = signal(true);
  protected readonly error = signal<string | null>(null);

  protected readonly showForm = signal(false);
  protected readonly editingPurchaseOrder = signal<PurchaseOrder | null>(null);
  protected readonly formError = signal<string | null>(null);
  protected readonly formLoading = signal(false);
  protected readonly deleteConfirm = signal<PurchaseOrder | null>(null);
  protected readonly deleteLoading = signal(false);

  protected readonly totalPages = computed(() => {
    const total = this.totalRecords();
    const size = this.pageSize();
    if (size <= 0) {
      return 1;
    }
    return Math.max(1, Math.ceil(total / size));
  });

  protected readonly statusOptions = ['Draft', 'Approved', 'Shipped', 'Completed', 'Cancelled'];

  constructor(private readonly poService: PurchaseOrderService) {}

  ngOnInit(): void {
    this.loadPurchaseOrders();
  }

  protected onSupplierChange(event: Event): void {
    const value = (event.target as HTMLInputElement)?.value ?? '';
    this.supplier.set(value);
  }

  protected onStatusChange(event: Event): void {
    const value = (event.target as HTMLSelectElement)?.value ?? '';
    this.status.set(value);
  }

  protected onStartDateChange(event: Event): void {
    const value = (event.target as HTMLInputElement)?.value ?? '';
    this.startDate.set(value);
  }

  protected onEndDateChange(event: Event): void {
    const value = (event.target as HTMLInputElement)?.value ?? '';
    this.endDate.set(value);
  }

  protected onSortByChange(event: Event): void {
    const value = (event.target as HTMLSelectElement)?.value as PurchaseOrderQuery['sortBy'];
    if (value) {
      this.sortBy.set(value);
    }
  }

  protected onSortDirChange(event: Event): void {
    const value = (event.target as HTMLSelectElement)?.value as PurchaseOrderQuery['sortDir'];
    if (value) {
      this.sortDir.set(value);
    }
  }

  protected onPageSizeChange(event: Event): void {
    const value = Number((event.target as HTMLSelectElement)?.value ?? 10);
    this.pageSize.set(value);
    this.page.set(1);
    this.loadPurchaseOrders();
  }

  protected onApplyFilters(event?: Event): void {
    event?.preventDefault();
    this.page.set(1);
    this.loadPurchaseOrders();
  }

  protected onNextPage(): void {
    if (this.page() < this.totalPages()) {
      this.page.update((current) => current + 1);
      this.loadPurchaseOrders();
    }
  }

  protected onPrevPage(): void {
    if (this.page() > 1) {
      this.page.update((current) => current - 1);
      this.loadPurchaseOrders();
    }
  }

  protected onAddNew(): void {
    this.editingPurchaseOrder.set(null);
    this.formError.set(null);
    this.showForm.set(true);
  }

  protected onEdit(po: PurchaseOrder): void {
    this.editingPurchaseOrder.set(po);
    this.formError.set(null);
    this.showForm.set(true);
  }

  protected onFormSubmit(dto: CreatePurchaseOrderDto | UpdatePurchaseOrderDto): void {
    const po = this.editingPurchaseOrder();
    this.formLoading.set(true);
    this.formError.set(null);

    if (po) {
      // Update
      this.poService.updatePurchaseOrder(po.id, dto as UpdatePurchaseOrderDto).subscribe({
        next: () => {
          this.showForm.set(false);
          this.editingPurchaseOrder.set(null);
          this.formLoading.set(false);
          this.loadPurchaseOrders();
        },
        error: (err) => {
          this.formError.set(err?.message || 'Failed to update purchase order');
          this.formLoading.set(false);
        },
      });
    } else {
      // Create
      this.poService.createPurchaseOrder(dto as CreatePurchaseOrderDto).subscribe({
        next: () => {
          this.showForm.set(false);
          this.formLoading.set(false);
          this.loadPurchaseOrders();
        },
        error: (err) => {
          this.formError.set(err?.message || 'Failed to create purchase order');
          this.formLoading.set(false);
        },
      });
    }
  }

  protected onFormCancel(): void {
    this.showForm.set(false);
    this.editingPurchaseOrder.set(null);
    this.formError.set(null);
  }

  protected onDeleteClick(po: PurchaseOrder): void {
    this.deleteConfirm.set(po);
  }

  protected onDeleteConfirm(): void {
    const po = this.deleteConfirm();
    if (!po) return;

    this.deleteLoading.set(true);
    this.poService.deletePurchaseOrder(po.id).subscribe({
      next: () => {
        this.deleteConfirm.set(null);
        this.deleteLoading.set(false);
        this.loadPurchaseOrders();
      },
      error: (err) => {
        this.error.set(err?.message || 'Failed to delete purchase order');
        this.deleteLoading.set(false);
      },
    });
  }

  protected onDeleteCancel(): void {
    this.deleteConfirm.set(null);
  }

  private loadPurchaseOrders(): void {
    this.loading.set(true);
    this.error.set(null);

    const query: PurchaseOrderQuery = {
      supplier: this.supplier() || undefined,
      status: this.status() || undefined,
      startDate: this.startDate() || undefined,
      endDate: this.endDate() || undefined,
      sortBy: this.sortBy(),
      sortDir: this.sortDir(),
      page: this.page(),
      pageSize: this.pageSize(),
    };

    this.poService.getPurchaseOrders(query).subscribe({
      next: (res) => {
        this.purchaseOrders.set(res.data ?? []);
        this.totalRecords.set(res.totalRecords ?? 0);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set(err?.message || 'Failed to load purchase orders');
        this.loading.set(false);
      },
    });
  }
}
