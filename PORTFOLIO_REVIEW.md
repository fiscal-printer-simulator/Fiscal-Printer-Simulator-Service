# Portfolio Review – Fiscal Printer Simulator (2026)

> **TL;DR – Yes, keep it. But invest a few hours to modernise the presentation.**

---

## 1. What the project demonstrates

| Skill | Where it shows up |
|---|---|
| **Domain modelling** | `IFiscalPrinter` / `BaseCommandHandler` abstraction lets any protocol be plugged in |
| **Plugin architecture** | Assembly scanning at startup (`Assembly.LoadFrom` + interface discovery) |
| **Binary protocol parsing** | Regex-based Posnet Thermal frame splitter (`ThermalFiscalPrinter.SplitByCommands`) |
| **Windows Services** | `ServiceBase` lifecycle (`OnStart` / `OnStop`), installer class |
| **Real-time communication** | WebSocket server (Fleck) pushing Redux-style actions to a browser client |
| **Unit testing** | NUnit 3 + Moq + FluentAssertions; per-command handler tests |
| **Multi-project solution** | Base library → protocol plugin → service → test project |

This is a **non-trivial, domain-specific tool** with a clear real-world use case (testing POS
integrations without physical hardware). Hiring managers and senior engineers will recognise the
value of such a tool — it shows you understood the *pain* of the job and built something to fix it.

---

## 2. What is strong ✅

- **Clean interface boundary.** `IFiscalPrinter` + `CommandHandlerResponse` means adding Posnet
  Standard or Novitus XML requires zero changes to the service layer.
- **Tested command handlers.** Most of the business logic (PTU rate changes, cashier
  login/logout, clock setup, slip printing) has dedicated unit tests. That matters more than
  coverage numbers.
- **Purposeful use of Redux over WebSocket.** Pushing typed action objects to the client is a
  pragmatic design that mirrors what a real front-end (React + Redux) expects.
- **Self-contained default state.** `FiscalPrinterState` initialises sane defaults so the
  simulator is ready to use out of the box without extra configuration.

---

## 3. What to improve before showing it in 2026 🔧

### 3a. Already fixed in this PR
- ✅ Replaced stale Travis CI badge with GitHub Actions (`ci.yml`)
- ✅ Fixed incorrect README statement ("dotnet core 2.2" — the project targets **net472**)
- ✅ Corrected recurring spelling mistakes in public identifiers:
  `AvaliblePortsAction` → `AvailablePortsAction`,
  `OutputReciptBuffer` → `OutputReceiptBuffer`,
  `ReciptWidth` → `ReceiptWidth`,
  `FeedPapperCommandHandler` → `FeedPaperCommandHandler`,
  `SetupReciptHeaderCommandHandler` → `SetupReceiptHeaderCommandHandler`,
  `SetupPaperKnifeHeightAndClientDisplayCommandHander` → `…CommandHandler`
- ✅ Updated README with architecture diagram, correct prerequisites and a protocol status table

### 3b. Quick wins (1–2 hours each)

| Issue | Recommendation |
|---|---|
| `System.Diagnostics.Debugger.Launch()` in the constructor | Remove the `#if DEBUG` block — it pops a dialog on any debug build, which is surprising |
| Plugin loader silently ignores `plugins.Count() != 1` | Log a warning and throw if zero plugins are found; allow multiple plugins |
| `catch { }` swallows all exceptions in `ThermalFiscalPrinter.HandleReceivedData` | At minimum log the exception; consider re-throwing or returning an error response |
| `ArgumentNullException("portName", …)` in dispatcher | Use `nameof(portName)` for the parameter name argument |
| `Debugger.Launch()` + `Console.ReadKey()` (commented out) | Remove commented-out code |
| `TimeDiffrenceInMinutes` field name | Fix spelling to `TimeDifferenceInMinutes` |
| `NextFiscalPrinterReciptId` field | Fix to `NextFiscalPrinterReceiptId` |

### 3c. Medium investment (portfolio differentiator)

| Item | Why it matters |
|---|---|
| **Migrate to .NET 8 / net8.0-windows** | net472 works but signals "legacy". .NET 8 targets run on Windows and support `System.IO.Ports` + `System.ServiceProcess`. Tests and library code would need zero logic changes. |
| **Add a GitHub Actions release workflow** | Trigger the MSI build (Setup-Builder repo) on tag push; distribute via GitHub Releases |
| **Add a `docker-compose` / `README` screenshot** | A GIF or screenshot of the Client displaying a receipt is the single most effective thing you can add to a portfolio project |
| **Implement one more protocol** | Even a partial Posnet Standard implementation signals the plugin architecture actually works for multiple protocols |
| **Add integration test** | One test that wires `ThermalFiscalPrinter` through a full command sequence (login → slip lines → approve) would demonstrate end-to-end confidence |

---

## 4. Is it portfolio-appropriate for a .NET / React developer in 2026?

**Yes.** Here is why:

1. **It is the right size.** Big enough to show real engineering decisions (interfaces, plugins,
   binary protocols, WebSocket pub/sub) but small enough to explain in a 15-minute interview.

2. **It shows hardware/OS integration.** Most web developers have never touched serial ports or
   Windows Services. This immediately differentiates your profile.

3. **It is a genuine tool, not a tutorial project.** You built it to solve a real problem at a
   real job. That story resonates.

4. **The React companion client ties it to your web stack.** You can present the full picture:
   C# service ↔ WebSocket ↔ React front-end.

### What to say in an interview

> "I worked on a POS application where testing was blocked by a limited number of physical
> fiscal printers. I built a Windows service that emulates the Posnet Thermal serial protocol
> and streams receipt data over WebSocket to a React dashboard. The service uses a plugin model
> so adding new protocols (Posnet Standard, Novitus XML) is a matter of dropping a new DLL."

---

## 5. Organisation-wide notes

| Repository | Status | Recommendation |
|---|---|---|
| **Fiscal-Printer-Simulator-Service** (this repo) | Active | Apply items in §3 above |
| **Fiscal-Printer-Simulator-Client** | Active | Check for stale JS dependencies; add a screenshot to the README |
| **Fiscal-Printer-Simulator-Setup-Builder** | Active (last updated 2026) | Wire into GitHub Actions release pipeline |
| **Fiscal-Printer-Comunication-Libraries** | Stale (2025) | The repository name itself contains a spelling error ("Comunication" → "Communication"); clarify its relationship to the Service repo in the README; consider merging or archiving |
| **Fiscal-Printer-Diagnostic-Tool** | Abandoned (last updated 2019) | Archive or clearly mark as unmaintained in the README |
