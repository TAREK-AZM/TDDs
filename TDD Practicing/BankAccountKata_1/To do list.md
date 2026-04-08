# 🧾 Bank Account Kata – TODO (TDD)

## 🟢 Account Creation
- [ ] Create `BankAccount` class
- [ ] Initialize balance to `0` by default
- [ ] Support optional initial balance (default = 0)
- [ ] Ensure balance is never negative at creation

---

## 💰 Deposit Feature
### Behavior
- [ ] Deposit succeeds when amount is positive
- [ ] Increase balance correctly

### Validation
- [ ] Reject deposit when amount is negative or zero

### Side Effects
- [ ] Create a `Transaction` when deposit succeeds
- [ ] Add transaction to history

---

## 💸 Withdraw Feature
### Behavior
- [ ] Withdraw succeeds when:
  - [ ] Amount is positive
  - [ ] Amount ≤ balance
- [ ] Decrease balance correctly

### Validation
- [ ] Reject withdraw when amount is negative or zero
- [ ] Reject withdraw when amount > balance

### Side Effects
- [ ] Create a `Transaction` when withdraw succeeds
- [ ] Add transaction to history

---

## 📜 Transaction Management
- [ ] Create `Transaction` class
  - [ ] Date
  - [ ] Amount
  - [ ] Type (Deposit / Withdraw)
  - [ ] Balance after operation (optional)
- [ ] Maintain transaction history list in `BankAccount`

---

## 🖨️ Statement Printing
- [ ] Implement transaction history printing
- [ ] Order transactions by date ascending
- [ ] Format output as a table

---

## 📅 Date Handling
- [ ] Assign a date to each transaction
- [ ] Use consistent date format (e.g. `yyyy-MM-dd`)
- [ ] (Optional) Use a date provider for testability

---

## 🧠 Design Improvements
- [ ] Ensure methods follow Single Responsibility Principle
- [ ] Avoid duplication between deposit and withdraw logic
- [ ] Use meaningful method and variable names
- [ ] Keep tests small and focused (1 behavior per test)

---

## 🚀 TDD Implementation Order
- [ ] Create account (balance = 0)
- [ ] Deposit success
- [ ] Deposit failure
- [ ] Withdraw success
- [ ] Withdraw failure (negative)
- [ ] Withdraw failure (insufficient funds)
- [ ] Transaction history
- [ ] Statement printing
- [ ] Date handling