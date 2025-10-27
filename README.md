# 🧑‍💻 ZeilCardValidatorApi — Developer Guide

This document provides **in-depth technical guidance** for developers working on `ZeilCardValidatorApi`.  
It complements the main `README.md` by focusing on:
- 📂 project architecture & responsibilities  
- 🧠 business logic structure  
- 🧪 testing strategy  
- 🧰 dependency injection & middleware  
- 🚀 extensibility patterns

---

## 🏗️ 1. Architecture Overview

The project follows a **layered architecture** inspired by Clean Architecture.  
The goal is to keep **business logic independent** from infrastructure and presentation concerns.

```
ZeilCardValidatorApi
├─ Application           → Business logic, services, validation
├─ CardsValidatorApi     → API layer (controllers, middleware)
└─ Domain                → Core exceptions, domain entities
```

| Layer | Responsibilities | Depends On |
|-------|--------------------|------------|
| **Domain** | Core exceptions, domain rules | None |
| **Application** | Luhn algorithm, validation, service interfaces | Domain |
| **API** | Controllers, Middleware, DI, Hosting | Application |

### 🔑 Key Principles
- **Separation of concerns** — each layer has a single responsibility.
- **Dependency inversion** — higher layers depend on abstractions (interfaces), not implementations.
- **Domain independence** — the `Domain` layer is free from any external dependencies.

---

## 🧠 2. Business Logic (Application Layer)

### 📄 `ILuhnService` & `LuhnService`
Located in:
```
Application/Services/
```

This service implements the **Luhn algorithm** and card masking logic.

```csharp
public interface ILuhnService
{
    bool Validate(string rawCardNumber);
    string Mask(string rawCardNumber);
}
```

Implementation detail:
- Removes spaces and hyphens using a compiled regex.
- Checks digits only.
- Verifies length (8–19 digits).
- Runs Luhn algorithm validation.

---

## 🧰 3. Dependency Injection

All services are registered in `Program.cs` of the API layer:

**Guideline:**  
- All business logic should be resolved via interfaces.  
- No direct new-ing up of service classes inside controllers.

---

## 🧱 4. Middleware

File: `CardsValidatorApi/Middleware/ExceptionHandlingMiddleware.cs`

This global middleware:
- Catches unhandled exceptions
- Logs the error 
- 
💡 **Tip:** You can extend this middleware to log masked card numbers only.

---

## 🧭 5. Validation

The request model:
```csharp
public class ValidateRequest
{
    public string CardNumber { get; set; } = string.Empty;
}
```

Validator:
```csharp
public class CreditCardRequestValidator : AbstractValidator<ValidateRequest>
{
    public CreditCardRequestValidator()
    {
        RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("Card number is required.")
                .Length(13, 19).WithMessage("Card number must be between 13 and 19 digits.")
                .Must(card =>
                {
                    var digitsOnly = card.NormalizeCardNumber();
                    return digitsOnly.All(char.IsDigit);
                }).WithMessage("Card number must contain digits only.");
    }
}
```

📌 This runs before hitting the controller action. If validation fails, a `400 Bad Request` is automatically returned with problem details.

---

## 🌐 6. API Controller

File: `CardsValidatorApi/Controllers/CardsValidationController.cs`

---

## 🧪 7. Testing Strategy

### Planned Tools
- **xUnit** for unit tests
- **FluentAssertions** for clean, expressive assertions
- **Moq** for mocking (optional for future integrations)

```
tests/
└─ ZeilCardValidatorApi.Tests/
   ├─ Services/
   │   └─ LuhnServiceTests.cs
   └─ Validators/
       └─ CreditCardRequestValidatorTests.cs
```

---

## 🧭 8. Extensibility Guidelines

✅ **Future features planned:**
- Add credit card type detection (Visa/MasterCard) in Application layer.  
- Introduce authentication in API layer (API Key / OAuth).  
- Dockerize for container deployment.  
- Add integration tests for full request pipeline.

---

## 🐳 9. Deployment & Configuration

### Environment Config
- `appsettings.json` for configuration
- `launchSettings.json` for local development

---

## 📜 License

MIT License — free to use, modify, and distribute with attribution.

---

## 👥 Maintainers

- **Project:** ZeilCardValidatorApi  
- **Language:** C# / .NET 8  
- **Architecture:** Clean / Layered  
- **Documentation:** Swagger + Markdown
