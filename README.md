# 📦 Purchase Order Management System

A full-stack Purchase Order Management System built with **ASP.NET Core Web API** (Backend) and **Angular** (Frontend). This application allows users to create, view, update, delete, filter, sort, and paginate purchase orders.

<img width="2245" height="1148" alt="image" src="https://github.com/user-attachments/assets/66379515-eb4d-4838-9387-af5e5fdeef20" />
---

## 🛠️ Tech Stack

### Backend

* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* RESTful APIs

### Frontend

* Angular (Standalone Components)
* TypeScript
* HTML / CSS
* Angular Router
* Angular HttpClient

---

## ✨ Features

* Create new Purchase Orders
* View all Purchase Orders
* Filter by Supplier Name and Status
* Sort by Order Date, Supplier Name, or Total Amount
* Pagination support
* Update existing Purchase Orders
* Delete Purchase Orders
* REST API integration

---

## 📁 Project Structure

```
order-management/
│
├── backend/
│   └── PurchaseOrderAPI/
│       ├── Controllers/
│       ├── Models/
│       ├── Data/
│       ├── Enums/
│       └── DTOs
│
└── frontend/
    └── purchase-order-ui/
        ├── src/app/
        │   ├── services/
        │   ├── models/
        │   └── components/
        └── main.ts
```

---

## 🚀 Getting Started

### Prerequisites

* .NET 7 or later
* Node.js (LTS recommended)
* Angular CLI
* SQL Server / SQL Server Express

---

## 🔧 Backend Setup

1. Open the backend project in **Visual Studio**
2. Update `appsettings.json` with your SQL Server connection string
3. Run database migrations (if applicable)
4. Start the API

The API will run on:

```
https://localhost:7231
```

### Sample API Endpoint

```
GET /api/purchaseorders
```

---

## 🎨 Frontend Setup

1. Navigate to the frontend folder

```bash
cd frontend/purchase-order-ui
```

2. Install dependencies

```bash
npm install
```

3. Run Angular app

```bash
ng serve
```

Frontend will be available at:

```
http://localhost:4200
```

---

## 🔗 API Integration

Angular communicates with backend using `HttpClient`.

Example API response structure:

```json
{
  "totalRecords": 10,
  "page": 1,
  "pageSize": 10,
  "data": []
}
```

---

## 🧪 Testing

* Backend tested using **Postman**
* Frontend tested via browser and Angular DevTools

---




