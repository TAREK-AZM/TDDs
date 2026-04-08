# 🧾 Bank Account Kata – TODO (TDD)

## 🟢 Account Creation
- [ ] Create `BankAccount` class
- [ ] Initialize balance to `0` by default
- [ ] Support optional initial balance (default = 0)
- [ ] Ensure balance is never negative at creation

---

## 💰 Deposit Feature
### Behavior~
- [ ] Deposit succeeds when amount is positive
- [ ] Increase balance correctly
- [ ] Reject deposit when amount is negative or zero
- [ ] Create a `Transaction` when deposit succeeds
- [ ] Add transaction to history

---

## 💸 Withdraw Feature
### Behavior
- [ ] Withdraw succeeds when:
  - [ ] Amount is positive and greather than or equal Minimum Amount for withdraw
  - [ ] Amount ≤ balance
  - [ ] Decrease balance correctly
  - [ ] Reject withdraw when amount is negative or zero

  - [ ] Maximun withdrawal per day is 5000 Dh --> needs date filltering
  - [ ] reject Transaction when limit exceeded
  - [ ] Reject withdraw when |balance - amout| <= 100 DH if it has not sufficient balance < 100
  - [ ] Create a `Transaction` when withdraw succeeds
  - [ ] Add transaction to history
	
	
### OverDraft
  - [ ] is OvertDraft is allowed
  - [ ] Maximum limit -100 DH 
  - [ ] reject if it depassed the limit
  - [ ] Apply overdraft fees
  
### Fees
!!!! - [ ] when make withdraw with credit until -100Dh the next Deposit substract the -100Dh if possible

- [ ] Deduct fee automatically from balance.
- [ ] Record fee as a separate transaction

- [ ] withdraw fees:
   - [ ] fees on withdrawl on the same bank is 0
   - [ ] fees on withdrawal from diffents banks is 6DH
	
- [ ] Transfer fees :
   - [ ] fees of transfer to another account in the same bank  is 0
   - [ ] fees of transfer to another bank account in the same instance is 15 DH
   - [ ] fees of transfer to another bank account in the same instance is 0 DH after 24h
   
---

## 📜 Transaction Management
- [ ] Create `Transaction` class
  - [ ] Id ( each transaction has it unique Identity )
  - [ ] Date
  - [ ] Amount
  - [ ] Type (Deposit / Withdraw / Fees)
  - [ ] Balance after operation (optional)
  - [ ] Maintain transaction history list in `BankAccount`
	

## 🖨️ Statement Printing
- [ ] Implement transaction history printing
- [ ] Order transactions by date Descending
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
