export interface PurchaseOrder {
  id: number;
  poNumber: string;
  description: string;
  supplierName: string;
  orderDate: string;
  totalAmount: number;
  status: string;
}

export interface PurchaseOrderPage {
  totalRecords: number;
  page: number;
  pageSize: number;
  data: PurchaseOrder[];
}

export interface PurchaseOrderQuery {
  supplier?: string;
  status?: string;
  startDate?: string;
  endDate?: string;
  sortBy?: 'OrderDate' | 'SupplierName' | 'TotalAmount' | 'PoNumber';
  sortDir?: 'asc' | 'desc';
  page?: number;
  pageSize?: number;
}

export interface CreatePurchaseOrderDto {
  poNumber: string;
  description: string;
  supplierName: string;
  orderDate: string;
  totalAmount: number;
}

export interface UpdatePurchaseOrderDto extends CreatePurchaseOrderDto {
  status: string;
}
