# Tatum Bank — App A

### C# Console Banking Application with ASP.NET Core Minimal API and Centralized Bill Payment Processing

**Repository:** https://github.com/Naheemah-babs/TatumBank-v2

**Technology:** C# | .NET 10 | ASP.NET Core Minimal API | REST API | Dependency Injection | In-Memory Storage

**Partner Bank:** https://github.com/Naheemah-babs/BankB

---

## 1. Project Overview

Tatum Bank App A is an educational banking simulation built with **C# and .NET 10**. It combines an interactive console banking application with an ASP.NET Core Minimal API in one running process.

Customers use console menus while the HTTP server handles requests from another independently running bank.

The two-application system comprises:

- **App A (this repository):** A complete banking application and the centralized simulated airtime/data payment provider.
- **App B:** A separate banking application that can send and receive interbank transfers and sends airtime/data payment requests to App A.

Each application owns its own in-memory accounts. The banks exchange data using HTTP and JSON rather than sharing account collections or databases. Both can initiate and receive interbank transfers.

The project demonstrates object-oriented design, interfaces, dependency injection, asynchronous HTTP communication, validation, transaction references, idempotency, concurrency protection, and handling of uncertain distributed operations.

## 2. Key Features

### Customer Banking

Customers can:

- Create accounts with a minimum opening deposit of **₦1,000**.
- Log in using their account number and PIN.
- Log out without closing the program.
- Deposit and withdraw funds.
- Transfer funds within App A (intrabank).
- Send money to App B (interbank).
- Receive transfers from App B.
- Purchase simulated airtime and mobile data.
- View balances and transaction history.
- Receive validation, success, failure, and pending messages.

### Administrative Operations

Administrators can:

- Freeze and unfreeze customer accounts.
- Inspect account status.
- Configure simulated bill-payment outcomes: Random, Force Success, or Force Failure.
- Exit the admin session without terminating the application.

A frozen account may log in and receive incoming transfers but cannot initiate withdrawals, outgoing transfers, or bill payments.

### Centralized Bill-Payment Provider

App A can:

- Accept airtime and data requests over HTTP.
- Validate payment details, supported networks, and configured data plans.
- Return simulated success or failure results.
- Store processed payment results by reference to prevent duplicate processing.
- Reject reused references containing conflicting request details.
- Support retry-based reconciliation by connected banks.

## 3. Technology Stack

| Technology | Purpose |
|---|---|
| C# | Banking logic and application code |
| .NET 10 | Runtime and application framework |
| ASP.NET Core Minimal API | Lightweight HTTP endpoints |
| Console Application | Customer and administrator interaction |
| HttpClient | Interbank and provider HTTP requests |
| Dependency Injection | Registration and resolution of services |
| Interfaces | Repository and HTTP-client contracts |
| DTOs and JSON | Structured requests and responses |
| In-memory Collections | Account, transaction, and processed-request storage |
| Locks / Concurrency Primitives | Protection of shared mutable state |
| Background Services | Periodic reconciliation of uncertain transfers, when enabled |
| Visual Studio 2026 | Development and debugging |
| Postman | Manual API testing |

**No external database is required.**

## 4. Project Architecture

The application follows a layered architecture that separates customer interaction, business rules, data management, and HTTP communication.

### Repository Structure

```text
TatumBank-v2/
│
├── BLL/
│   └── Banking, transfers, and payment business logic
│
├── Controllers/
│   └── ASP.NET Core endpoint mappings
│
├── DTOs/
│   └── Request and response types
│
├── Interfaces/
│   └── Repository and HTTP-client contracts
│
├── Models/
│   └── Accounts, transactions, and enums
│
├── Presentation/
│   └── Console menus and input handling
│
├── Repositories/
│   └── In-memory account repository
│
├── Services/
│   └── HTTP clients and supporting/background services
│
├── Properties/
│   └── Development launch settings (if present)
│
├── Program.cs
├── TatumWelcome2.csproj
└── *.slnx
```

### Layer Responsibilities

**Presentation Layer**

Handles customer and administrator menus, user input, and validation feedback.

**Business Logic Layer (BLL)**

Applies banking rules for accounts, deposits, withdrawals, transfers, and payments.

**Models**

Represent account and transaction data and provide methods for managing account state.

**Repositories**

Provide account lookup and in-memory storage access.

**Interfaces**

Define contracts for repositories and communication services.

**DTOs**

Define the information exchanged between banks through HTTP.

**Controllers**

Map HTTP routes to the appropriate business operations.

**Services**

Provide HTTP communication and supporting background operations.

### Dependency Injection

The application registers its in-memory repository and business services in `Program.cs`.

For example, the HTTP client is registered using:

```csharp
builder.Services.AddHttpClient<IBankApiClient, BankApiClient>();
```

Business services use singleton registrations where shared in-memory state must be retained.

The console resolves the services it needs from the application's dependency injection container.

## 5. Application Execution

App A's console interface and HTTP API run within the same application process.

### Execution Flow

1. Configure UTF-8 console output and Nigerian currency culture.
2. Create the ASP.NET Core application builder.
3. Load the initial in-memory accounts.
4. Register repositories, HTTP clients, and banking services.
5. Map health, account, transfer, and bill-payment endpoints.
6. Start the HTTP server asynchronously using `app.RunAsync()`.
7. Run the console banking menus.
8. Stop the HTTP server when the console application exits.

### Default Application Addresses

| Application | Local URL | Role |
|---|---|---|
| App A | `http://localhost:5000` | Bank and centralized bill-payment provider |
| App B | `http://localhost:5001` | Second independent bank |

Run both applications simultaneously for cross-bank tests.

An application can still perform its local banking operations when the other bank is offline.

## 6. Customer Banking Operations

### Account Creation

New customers provide their account details and an opening deposit of at least **₦1,000**.

The application assigns an account number and includes the opening deposit in the customer's account balance.

### Login and Logout

Customers authenticate using their account number and PIN.

Logging out returns the customer to the main menu without stopping the console application or HTTP API.

### Deposits and Withdrawals

Deposits increase the account balance and create a transaction record.

Withdrawals require:

- A valid withdrawal amount.
- Sufficient available funds.
- An account permitted to transact.
- Successful PIN validation where required.

### Intrabank Transfers

A customer can transfer money to another account held within App A.

The application validates the sender, recipient, amount, and available funds before recording the corresponding outgoing and incoming transactions.

### Interbank Transfers

A customer enters the recipient account number, selects App B, and authorizes the transfer.

App A communicates with App B over HTTP rather than directly accessing App B's account records.

### Airtime and Data Purchases

Customers select:

- Mobile network.
- Recipient phone number.
- Airtime amount or data plan.

App A processes simulated payments through its centralized provider functionality.

### Transaction History

Transaction records contain information such as:

- Transaction type.
- Amount.
- Date.
- Description.
- Transaction reference, where applicable.

Customers can review their transaction history from the console.

## 7. Bidirectional Interbank Transfers

Both applications can initiate and receive interbank transfers.

### Communication Flow

```text
Bank A                               Bank B
  |                                    |
  |-------- HTTP transfer request ---->|
  |<------- HTTP response -------------|
  |                                    |
  |<------- HTTP transfer request -----|
  |-------- HTTP response ------------>|
```

### Example: App A Sends ₦5,000 to App B

1. App A validates the sender's PIN, amount, and balance.
2. App A generates a unique transfer reference.
3. App A debits the sender.
4. App A sends the transfer details to App B's receiving endpoint.
5. App B validates the request and locates the recipient.
6. App B credits the recipient once.
7. App B returns an HTTP response.
8. App A reports the result or marks it uncertain if confirmation is unavailable.

### Example: App B Sends Money to App A

The same process works in reverse.

App B becomes the sending bank, while App A receives and processes the incoming transfer.

App A must expose the receiving and transfer-status endpoints used by App B.

### Duplicate-Transfer Protection

The receiving service stores processed transfer references.

- **Identical reference and details:** Return an already-processed result without crediting again.
- **Same reference with different details:** Reject the request as a reference conflict.
- **New valid reference:** Process the transfer and record its details.

This helps prevent duplicate incoming credits.

## 8. API Endpoints

**Base URL:** `http://localhost:5000`

| Method | Endpoint | Description |
|---|---|---|
| GET | `/health` | Checks whether the API is running |
| GET | `/accounts/{number}` | Retrieves recipient account lookup information |
| POST | `/transfers/receive` | Processes an incoming interbank transfer |
| GET | `/transfers/status/{reference}` | Checks a processed incoming transfer |
| POST | `/bills/pay` | Processes simulated airtime/data payments |
| GET | `/bills/networks` | Retrieves supported mobile networks |
| GET | `/bills/plans` | Retrieves configured data plans |

**Route verification:** The latest transfer controller shared during development uses `/transfers/receive` and `/transfers/status/{reference}`. An earlier App A version used `/interbank/transfer`. Confirm the routes in the currently compiled `InterbankTransferEndpoints.cs` before publishing or testing.

### 8.1 Health Check

**Endpoint**

```http
GET http://localhost:5000/health
```

**Purpose:** Confirms that App A's HTTP API is running.

**Expected response**

```text
Tatum Bank API is running
```

### 8.2 Account Lookup

**Endpoint**

```http
GET http://localhost:5000/accounts/0012345678
```

**Purpose:** Retrieves account lookup information needed for recipient verification.

The API returns a DTO rather than the complete internal account object.

Sensitive account information, including PINs, should not be exposed.

### 8.3 Incoming Interbank Transfer

**Endpoint**

```http
POST http://localhost:5000/transfers/receive
```

**Content-Type:** `application/json`

**Example request**

```json
{
  "reference": "TRF-001",
  "senderAccountNumber": "0087654321",
  "recipientAccountNumber": "0012345678",
  "amount": 5000
}
```

### HTTP Response Codes

| Status | Meaning |
|---|---|
| 200 OK | Transfer processed or identical request already processed |
| 400 Bad Request | Invalid transfer request |
| 404 Not Found | Recipient account not found |
| 409 Conflict | Reference reused with different transfer details |

### 8.4 Transfer Status

**Endpoint**

```http
GET http://localhost:5000/transfers/status/TRF-001
```

**Purpose:** Checks whether App A has processed an incoming transfer.

**200 OK:** Returns the processed transfer's reference, sender, recipient, and amount.

**404 Not Found:** No processed transfer exists for the reference at the time of the check.

A `404` does not prove that an earlier transfer request can never arrive.

### 8.5 Bill Payment

**Endpoint**

```http
POST http://localhost:5000/bills/pay
```

**Content-Type:** `application/json`

**Example request**

```json
{
  "reference": "BILL-001",
  "bankId": "BankB",
  "accountNumber": "0033333333",
  "paymentType": "Airtime",
  "network": "MTN",
  "phoneNumber": "08012345678",
  "amount": 100,
  "dataPlanName": null
}
```

**Successful response**

```json
{
  "success": true,
  "message": "Bill payment successful.",
  "reference": "BILL-001"
}
```

**Example reference-conflict response**

```json
{
  "success": false,
  "message": "Payment reference already used for another request.",
  "reference": "BILL-001"
}
```

### 8.6 Supported Networks

**Endpoint**

```http
GET http://localhost:5000/bills/networks
```

**Purpose:** Returns the mobile networks supported by the payment provider.

Configured networks include:

- MTN
- Airtel
- Glo
- 9mobile

### 8.7 Available Data Plans

**Endpoint**

```http
GET http://localhost:5000/bills/plans
```

**Purpose:** Returns the data plans configured in the simulated payment provider.

The exact plans depend on the application's current configuration.

## 9. Centralized Bill-Payment Processing

App A acts as the simulated provider for airtime and data purchases.

App B sends requests to App A rather than independently determining payment outcomes.

### Payment Workflow

```text
Customer selects airtime or data
              |
              v
Bank validates payment details
              |
              v
Generate unique payment reference
              |
              v
Send POST /bills/pay to App A
              |
              v
App A validates payment request
              |
              v
Check payment reference
              |
      +-------+-------+
      |               |
      v               v
 New reference    Known reference
      |               |
      v               v
Process payment   Compare details
      |               |
      v         +-----+-----+
Store result     |           |
      |          v           v
      v       Identical   Different
Return result    |           |
                 v           v
             Return old    Reject
               result     conflict
```

### Airtime Purchases

The customer selects a mobile network, recipient phone number, and amount.

The configured minimum airtime purchase is **₦50**.

### Data Purchases

The customer selects a mobile network, recipient phone number, and an available data plan.

The selected plan supplies the purchase details.

### Nigerian Phone Number Validation

Phone numbers must satisfy these rules:

- Exactly 11 digits.
- Numeric characters only.
- Accepted prefixes: `070`, `080`, `081`, `090`, and `091`.

**Valid example:** `08012345678`

**Invalid example:** `09876543211`

## 10. Bill-Payment Idempotency

Idempotency prevents repeated requests from processing the same payment more than once.

App A uses a unique payment reference to identify a payment attempt.

The provider stores the processed outcome and compares subsequent requests using the same reference.

### Identical Request

If a request uses an existing reference with identical details, App A returns the previously stored result.

### Conflicting Request

If the reference exists but a material detail changes, App A rejects the request.

### Example

```text
First request:
Reference: TEST-DUP-001
Amount: ₦100
Result: Processed

Second request:
Reference: TEST-DUP-001
Amount: ₦100
Result: Original response returned

Third request:
Reference: TEST-DUP-001
Amount: ₦200
Result: Reference conflict
```

This is particularly important when App B retries a payment after losing the original HTTP response.

## 11. Administrative Payment Simulation Modes

App A provides administrative controls for simulating different payment outcomes.

| Mode | Behavior |
|---|---|
| Random | Approximately 80% success for new valid requests |
| Force Success | Approves new valid payment requests |
| Force Failure | Rejects new valid payment requests |

### Random Mode

Simulates variable payment outcomes during testing.

### Force Success

Used to test successful payment processing and account deductions.

### Force Failure

Used to test unsuccessful payments and the release of reserved funds in the requesting bank.

**Important:** Changing payment modes should not change the stored outcome of a previously processed reference.

These controls make success, failure, and recovery scenarios reproducible during testing.

## 12. Uncertain Bill Payments and Reconciliation

An HTTP timeout does not necessarily mean that a payment failed.

For example, App B may send a payment request, App A may process it successfully, and the HTTP response may be lost.

### Example Scenario

1. A customer initiates an airtime purchase through App B.
2. App B reserves the payment amount.
3. App B sends the payment request to App A.
4. App A processes the request.
5. The HTTP response is lost.
6. App B cannot confirm the payment outcome.
7. App B keeps the payment pending.
8. App B retries using the original payment reference.
9. App A returns the stored result.
10. App B finalizes the payment outcome.

### Reconciliation Outcomes

| Confirmed Outcome | Requesting Bank Action |
|---|---|
| Success | Finalize the payment and deduct the reserved amount once |
| Failure | Release the reservation without charging the customer |
| Unconfirmed | Keep the reservation pending and retry later |

The discussed background reconciliation interval is approximately **15 seconds**.

This mechanism depends on the applications remaining operational and their in-memory state remaining available.

## 13. Uncertain Interbank Transfers and Reconciliation

An interbank transfer may be debited at the sending bank and credited at the receiving bank even when the sender does not receive the HTTP response.

This creates an uncertain transaction outcome.

### Reconciliation Design

The updated reconciliation design for both banks is:

1. Generate a unique transfer reference.
2. Debit the sender once.
3. Send the transfer to the receiving bank.
4. If confirmation is lost, record the transfer as pending.
5. Retain its reference, receiving bank, recipient, and amount.
6. Periodically call the receiving bank's status endpoint.
7. Compare the returned details with the original transfer.
8. If the details match, confirm the transfer without another debit.
9. If the receiving bank is unavailable or returns `404`, keep the transfer pending.

### Transfer Status Endpoint

```http
GET /transfers/status/{reference}
```

### Confirmed Success

If the receiving bank confirms that the transfer was processed with matching details, the sending bank marks the transfer as confirmed.

No additional debit or credit is required.

### Unconfirmed Outcome

If the receiving bank cannot be reached or returns `404`, the transfer remains pending.

A missing status record does not automatically justify refunding the sender.

### Important Limitations

The reconciliation implementation discussed during development checks transfer status but does not automatically resend a missing transfer.

Therefore, a transfer that never reached the receiving bank can remain pending and require manual resolution.

Pending records are stored in memory, so recovery after an application restart is not guaranteed.

**Implementation status:** Verify that `InterbankReconciliationService` is registered in both applications and that the updated code has been built and tested before presenting this functionality as verified.

## 14. Account Freezing, Validation, and Error Handling

### Frozen Account Rules

A frozen customer:

- Can log in.
- Can receive incoming transfers.
- Cannot withdraw money.
- Cannot initiate outgoing transfers.
- Cannot purchase airtime.
- Cannot purchase data.

### Input Validation

The application validates:

- Required customer information.
- Account numbers.
- Minimum ₦1,000 opening deposit.
- PINs for protected operations.
- Positive transaction amounts.
- Sufficient available balance.
- Frozen-account restrictions.
- Receiving bank and recipient details.
- Supported mobile networks.
- Configured data plans.
- Nigerian phone number format.
- Unique and consistent payment references.
- Unique and consistent transfer references.

### Communication Failures

The application distinguishes between:

**Confirmed rejection:** The receiving service explicitly rejects a transaction.

**Uncertain outcome:** The application cannot determine whether the remote operation succeeded.

A transfer-status lookup returning `404` is not sufficient grounds for refunding an interbank transfer that may still be in transit.

## 15. Installation and Setup

### Prerequisites

Install:

- .NET 10 SDK.
- Visual Studio 2026 or another compatible .NET IDE.
- Git.
- Postman (optional).

### Clone App A

```bash
git clone https://github.com/Naheemah-babs/TatumBank-v2.git
```

### Navigate to the Project

```bash
cd TatumBank-v2
```

### Open in Visual Studio

Open the `.slnx` solution file included in the repository.

### Restore Dependencies

Visual Studio can restore dependencies automatically.

Alternatively:

```bash
dotnet restore
```

### Run App A

```bash
dotnet run --project TatumWelcome2.csproj
```

Ensure App A listens on:

```text
http://localhost:5000
```

### Run App B Separately

Clone the second bank:

```bash
git clone https://github.com/Naheemah-babs/BankB.git
```

Navigate to its directory:

```bash
cd BankB
```

Restore dependencies:

```bash
dotnet restore
```

Run the project:

```bash
dotnet run --project TatumWelcome2.csproj
```

Bank B should listen on:

```text
http://localhost:5001
```

Run both projects in separate terminals or Visual Studio instances.

If either repository uses a different project-file location, use the actual `.csproj` path.

### Verify Both APIs

**App A**

```http
GET http://localhost:5000/health
```

**App B**

```http
GET http://localhost:5001/health
```

Use the console menus for normal customer and administrative interactions.

Use Postman for direct API testing.

## 16. Testing

The application supports testing through the console interface and HTTP endpoints.

### Console Testing

Relevant scenarios include:

- Account creation and login.
- Deposits and withdrawals.
- Intrabank transfers.
- Interbank transfers in both directions.
- Account freezing and unfreezing.
- Airtime and data purchases.
- Transaction history.
- Invalid inputs.
- Insufficient available funds.

### Bill-Payment Testing

| Scenario | Expected Outcome |
|---|---|
| Successful airtime purchase | Payment approved and recorded |
| Failed airtime purchase | No permanent deduction |
| Successful data purchase | Correct amount deducted |
| Failed data purchase | No permanent deduction |
| Duplicate payment reference | Original response returned |
| Same reference with different details | Request rejected |
| Provider unavailable | Payment remains pending |
| Pending payment later succeeds | Payment reconciled and deducted once |
| Pending payment later fails | Reserved funds released |

### Interbank Transfer Testing

| Scenario | Expected Outcome |
|---|---|
| App A sends to App B | Recipient credited once |
| App B sends to App A | Recipient credited once |
| Invalid recipient account | Transfer rejected |
| Insufficient funds | Transfer rejected |
| Frozen sender | Outgoing transfer rejected |
| Frozen recipient | Incoming transfer permitted |
| Duplicate incoming request | No second credit |
| Conflicting transfer reference | Conflict returned |
| Network timeout | Outcome treated as uncertain |
| Lost response after credit | Status lookup confirms the transfer |
| Status lookup returns 404 | Transfer remains unresolved |

### Postman Testing: Bill-Payment Idempotency

**Test 1: First Payment**

```text
Reference: TEST-DUP-001
Amount: ₦100
```

Expected successful response:

```json
{
  "success": true,
  "message": "Bill payment successful.",
  "reference": "TEST-DUP-001"
}
```

**Test 2: Duplicate Request**

Submit the identical request again.

Expected outcome: The original response is returned without processing another payment.

**Test 3: Reference Conflict**

Submit the same reference with the amount changed to ₦200.

Expected response:

```json
{
  "success": false,
  "message": "Payment reference already used for another request.",
  "reference": "TEST-DUP-001"
}
```




## 17. Software Engineering Concepts Demonstrated

### Object-Oriented Programming

Classes and objects represent accounts, transactions, services, and other banking components.

### Encapsulation

Account balances and transaction operations are managed through defined methods and properties.

### Interfaces

Interfaces establish contracts between components, including repositories and HTTP communication services.

### Dependency Injection

Services are registered with the .NET dependency injection container and resolved where required.

### Asynchronous Programming

Asynchronous methods support HTTP communication and background operations.

### REST API Development

Minimal API endpoints enable communication between independently running banking applications.

### Data Transfer Objects

DTOs define structured information exchanged through HTTP requests and responses.

### Idempotency

Transaction references help prevent duplicate processing of payment requests and incoming transfers.

### Concurrency Control

Synchronization protects shared in-memory account and transaction state during concurrent operations.

### Background Processing

Background services support periodic checks of uncertain transactions.

### Separation of Concerns

Presentation, business logic, data access, models, and HTTP communication are organized into separate components.

### Error Handling

Validation and exception handling help the application respond to invalid input, failed transactions, and communication problems.

