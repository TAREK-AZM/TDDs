# 📘 Test-Driven Development By Example
## Section I: The Money Example

This project follows the first section of *Test-Driven Development By Example* by Kent Beck.
It demonstrates how to build a multi-currency arithmetic system using the core principles of Test-Driven Development (TDD).

## 🚀 Purpose

The goal of this project is to learn TDD by doing, not just by theory.

We progressively build a system that supports:

- Money representation (Dollar, Franc, etc.)
- Arithmetic operations (multiplication, addition)
- Currency conversion

All while strictly following the TDD cycle:

- 🔴 **Red** → Write a failing test
- 🟢 **Green** → Make it pass quickly
- 🔵 **Refactor** → Improve the design

## 🧠 Key Concepts Covered

- Test-Driven Development (TDD)
- Incremental design
- Refactoring
- Value Objects
- Polymorphism & abstraction
- Design patterns (Composite, Factory, etc.)

## 📚 Chapter Breakdown

### 🧩 Chapters 1–3: Initial Setup & Multiplication
- Introduces the business problem (multi-currency money handling)
- First test: money multiplication
- Uses the "Fake It" strategy (return constants to pass tests quickly)

> 💡 **Key idea:** Move fast to green, even with temporary hacks

### 💎 Chapters 4–6: Value Objects & Equality
- Introduces Value Objects to remove side effects
- Implements `equals()` properly
- Uses triangulation (generalize only after multiple examples)

> 💡 **Key idea:** Design evolves from concrete examples

### 💱 Chapters 7–9: Multi-Currency Support
- Adds a second currency (Franc)
- Reveals duplication between Dollar and Franc
- Introduces a shared abstraction: `Money`

> 💡 **Key idea:** Duplication drives abstraction

### 🧹 Chapters 10–13: Cleaning the Model
- Introduces Factory Methods (e.g., `Money.dollar()`, `Money.franc()`)
- Removes dependency on concrete classes
- Unifies representation using currency strings
- Deletes `Dollar` and `Franc` subclasses

> 💡 **Key idea:** Simplify the model by removing unnecessary classes

### ➕ Chapters 14–18: Addition & Metaphor
- Implements addition across currencies
- Introduces the `Expression` abstraction
- Uses:
  - Composite pattern (`Sum`)
  - Imposter pattern (treat `Money` as `Expression`)

> 💡 **Key idea:** Design flexibility through abstraction

### 🔍 Chapter 19: Retrospective
- Reviews the entire design and process
- Highlights:
  - The power of the `Expression` metaphor
  - TDD leads to cleaner, more flexible design
  - Typical 1:1 ratio of test code to production code

> 💡 **Key idea:** TDD shapes both design and thinking
