# Architecture

## Diagrams

Get all employees

```mermaid
sequenceDiagram
    autonumber
    Client ->> GET /api/employees: XHR Request (params)
    GET /api/employees ->> Employees (Cosmos): query (params)
    Employees (Cosmos) -->> GET /api/employees: employee entities
    GET /api/employees -->> Client: XHR Response (employees)
```

Create employee

```mermaid
sequenceDiagram
    autonumber
    Client ->> POST /api/employees: XHR Request (employee)
    POST /api/employees ->> Employees (Cosmos): (entity) employee
    Employees (Cosmos) -->> POST /api/employees: entity

    par
      POST /api/employees -->> Client: XHR Response (employee)
    and
      POST /api/employees ->> employee updates: message (entity)
      Note over POST /api/employees,employee updates: Storage Queue message
      employee updates ->> CreatePayrollFromQueue: dequeue
      CreatePayrollFromQueue ->> Employees (Cosmos): query (id)
      Employees (Cosmos) ->> CreatePayrollFromQueue: employee entity
      CreatePayrollFromQueue ->> Departments (Cosmos): (entity) department employee
      Departments (Cosmos) ->> CreatePayrollFromQueue: (entity) department employee
      CreatePayrollFromQueue ->> POST /api/notifications: department employee
      POST /api/notifications ->> Client: SignalR message (department employee)
    end
```
