import { ChangeDetectionStrategy, Component, input, output, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PurchaseOrder, CreatePurchaseOrderDto, UpdatePurchaseOrderDto } from '../../models/purchase-order.model';

@Component({
  selector: 'app-purchase-order-form',
  templateUrl: './purchase-order-form.html',
  styleUrl: './purchase-order-form.css',
  standalone: true,
  imports: [ReactiveFormsModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PurchaseOrderFormComponent {
  readonly purchaseOrder = input<PurchaseOrder | null>(null);
  readonly submit = output<CreatePurchaseOrderDto | UpdatePurchaseOrderDto>();
  readonly cancel = output<void>();

  protected form: FormGroup;
  protected readonly statusOptions = ['Draft', 'Approved', 'Shipped', 'Completed', 'Cancelled'];
  protected readonly submitting = signal(false);
  protected readonly errorMessage = signal<string | null>(null);

  constructor(private fb: FormBuilder) {
    this.form = this.createForm();
  }

  ngOnInit(): void {
    const po = this.purchaseOrder();
    if (po) {
      this.form.patchValue({
        poNumber: po.poNumber,
        description: po.description,
        supplierName: po.supplierName,
        orderDate: po.orderDate,
        totalAmount: po.totalAmount,
        status: po.status,
      });
    }
  }

  private createForm(): FormGroup {
    return this.fb.group({
      poNumber: ['', [Validators.required, Validators.minLength(1)]],
      description: ['', [Validators.required, Validators.minLength(1)]],
      supplierName: ['', [Validators.required, Validators.minLength(1)]],
      orderDate: ['', Validators.required],
      totalAmount: ['', [Validators.required, Validators.min(0.01)]],
      status: ['Draft'],
    });
  }

  protected onSubmit(): void {
    if (this.form.invalid) {
      this.errorMessage.set('Please fill in all required fields');
      return;
    }

    this.errorMessage.set(null);
    const po = this.purchaseOrder();
    const formValue = this.form.value;

    if (po) {
      // Update
      const updateDto: UpdatePurchaseOrderDto = {
        poNumber: formValue.poNumber,
        description: formValue.description,
        supplierName: formValue.supplierName,
        orderDate: formValue.orderDate,
        totalAmount: formValue.totalAmount,
        status: formValue.status,
      };
      this.submit.emit(updateDto);
    } else {
      // Create
      const createDto: CreatePurchaseOrderDto = {
        poNumber: formValue.poNumber,
        description: formValue.description,
        supplierName: formValue.supplierName,
        orderDate: formValue.orderDate,
        totalAmount: formValue.totalAmount,
      };
      this.submit.emit(createDto);
    }
  }

  protected onCancel(): void {
    this.cancel.emit();
  }
}
