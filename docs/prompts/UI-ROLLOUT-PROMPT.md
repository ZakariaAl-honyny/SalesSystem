# Ready-to-use prompt — roll the `frmProductMaster` standard out to the remaining screens

> Copy from "Prompt start" to "Prompt end" and paste it into any new session.
> Update only **§7 execution batches** with the screens assigned to that session.

---

## Prompt start

**Role:** WinForms implementation agent (.NET 10) inside the `SalesSystem.slnx` solution.
**Mission:** raise the screens in `SalesSystem.Desktop/Screens/**` to the level of the reference screen `frmProductMaster` — green theme + keyboard navigation + `gbFilter` + `pnlSummary` + standard grid + "design time == runtime".
**Highest authority:** `AGENTS.md` at the repository root. Any conflict between this prompt and `AGENTS.md` is resolved in favour of `AGENTS.md`.

### 0) Read before writing a single character

1. `AGENTS.md` — especially: §2.12 · §2.18 · §2.19 · §2.21 · §2.22 · §2.27 · §2.28 · §2.29 · §2.30 · §2.31 · §2.33 · §2.34 · §2.35 · §4.
2. The complete reference screen:
   - `SalesSystem/SalesSystem.Desktop/Screens/Products/Forms/frmProductMaster.cs`
   - `SalesSystem/SalesSystem.Desktop/Screens/Products/Forms/frmProductMaster.Designer.cs`
3. The helpers (the single source of shared behaviour — never copy them into a screen):
   - `SalesSystem/SalesSystem.Desktop/Helpers/KeyboardHelper.cs`
   - `.../Helpers/GreenThemeHelper.cs`
   - `.../Helpers/FilterPanelHelper.cs`
   - `.../Helpers/ThemeHelper.cs`
   - `.../Helpers/RowFilterHelper.cs`
   - `.../Helpers/LiveTafqeetHelper.cs`
   - `.../Helpers/ExportHelper.cs`
   - `.../Helpers/ReportUiStandardizer.cs` + `ReportNavigationHelper.cs` (reports only)
4. For the screen assigned to you: read **all three files in full** (`frmX.cs` + `frmX.Designer.cs` + `frmX.resx`) before any edit.

### 1) Anatomy of the reference screen (this is the visual and functional target)

| Block | Designer elements | Rule |
|---|---|---|
| Title bar | `lblMode` (Dock=Top) showing "Add … new" / "Edit …: <name>" | RULE-N44 |
| Editor | `pnlFields` + `Tahoma` labels + white input fields + `grpMainUnit` | §2.34 · RULE-N143 |
| Commands | `pnlButtons` (FlowLayoutPanel): New · Save · Edit · Delete · Reactivate · Manage units · Close | RULE-N43 |
| Content | `pnlGrid` + `dgvProducts` | RULE-N22 |
| Filtering | `pnlFilters` (Dock=Top, height 112) containing `btnToggleFilters` (Dock=Left, 140×104) + `gbFilter` (Dock=Fill, caption "بحث وتصفية" / "Search & filter", Padding=8) | RULE-N56…N63 · RULE-N145 |
| Summary | `pnlSummary` + `lblSummary` (`TextStrong`, Tahoma 10 Bold, height 32, "Results: N" and N2 totals) | RULE-N61 |

The vertical block order is **fixed**: title ← commands ← filters ← content ← totals ← footer (RULE-N135…N138). No box inside a box, and no block without a box.

### 2) Required work inside every screen — in order

1. **Structure:** reorder/group the blocks as in the table above while preserving existing control names and events (never break behaviour called from outside the screen).
2. **The six `KeyboardHelper` calls** in the constructor (RULE-N95):

```csharp
KeyboardHelper.WireEnterAsTab(this);
KeyboardHelper.WireSelectAllOnFocus(this);
KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0)); // RULE-N46
KeyboardHelper.WireArrowNavigation(this);                                   // RULE-N45
KeyboardHelper.WireButtonDisabledVisual(btnSave, btnEdit, btnDelete, btnReactivate); // RULE-N43
KeyboardHelper.WireGlobalShortcuts(this,                                    // RULE-N47
    (Keys.F2, () => btnNew.PerformClick()),
    (Keys.F5, () => btnRefresh.PerformClick()),
    (Keys.Delete, _GlobalDelete),
    (Keys.Escape, _GlobalEscape));
```

The last line of the constructor is always:

```csharp
FilterPanelHelper.Attach(this, _serviceProvider);   // RULE-N63
```

3. **State machine + `lblMode`** (entry/master screens only): `Browsing / Adding / Editing`; inputs disabled in `Browsing`; `_UpdateButtonsState()` + `_UpdateModeHeader()` on every transition.
4. **`gbFilter`** per §2.22: `cbFilterBy` + `txtFilterValue` (or `txtSearch` for single-search screens, as in the reference) · `cbStatus` · `dtpFrom`/`dtpTo` + `btnRangeToday`/`btnRangeMonth`/`btnRange30` · `chkShowInactive`/`chkOnlyDue`/`chkOnlyCancelled`/`chkOnlyOutOfStock` · `btnToggleFilters`. **Every filter control lives inside `gbFilter`, no exception** (RULE-N62).
5. **Instant filtering** on `TextChanged`/`CheckedChanged`/`SelectedIndexChanged` — there is no "Search" button; `Esc` clears the filters (RULE-N59). Any `RowFilter` is built exclusively through `RowFilterHelper.Like/Escape` on known columns (RULE-N24/N60) — never inject raw user input.
6. **`pnlSummary`:** result count + totals formatted `N2`, recomputed after every filter (RULE-N61).
7. **`DataGridView`:** `ReadOnly = true` · `AllowUserToAddRows = false` · `SelectionMode = FullRowSelect` · `ContextMenuStrip` for row operations · double-click · an empty-state "no data" row (RULE-N22/N23). The only two allowed exceptions: `dgvItems` in `frmAddUpdateSalesInvoice` and `dgvCart` in `frmPOS` (RULE-N22a/N83).
8. **Validation:** `errorProvider` + Arabic messages (RULE-N28). Never show `ex.Message` to the user — Serilog only (RULE-N29).
9. **Theme** — mandatory order inside `ApplyUiTheme()`:

```csharp
ThemeHelper.ApplyMainFormStyle(this);
ThemeHelper.ApplyButtonStyle(btnSave, ThemeHelper.ButtonType.Primary);
// ... the remaining buttons

GreenThemeHelper.ApplyHeader(lblMode);
GreenThemeHelper.ApplyGrid(dgvX);        // for every grid
GreenThemeHelper.Apply(this);            // ← always the last line (RULE-N50)
```

10. **Design time == runtime** (RULE-N134/N140…N144): every colour applied at runtime is declared with a matching literal in `*.Designer.cs`; input fields are white + `Tahoma`; `SystemColors.Control` and `UseVisualStyleBackColor = true` are forbidden on controls of green screens; no reference to any member of `Helpers/` inside the designer (RULE-N128).
11. **Money and quantities:** `decimal` displayed `N2`; quantities with 3 decimals (RULE-001/002).
12. **Live tafqeet** on money-entry screens: `LiveTafqeetHelper.Attach(liveTip, …)` (RULE-N97).
13. **Reactivation cycle** (RULE-N90…N94): the "Reactivate" button is enabled on the **inactive row only** + `chkShowInactive`.
14. **No dead code:** if the entity already has a `frm*Master`, a leftover `frmAddUpdate*`/`frmList*` for it is dead (RULE-N102) — **do not delete it without the manager's approval**, report it instead.
15. **Code language:** classic WinForms (code-behind), no ViewModel/ICommand, no new abstractions, no `HttpClient`, and no reference to `SalesSystem.Business` inside `Screens` (RULE-N109/N110/N111).
16. **Tokens:** no `async void` without `try/catch`, and no `Cursor.WaitCursor` without restoring it in `finally` (RULE-N25/N26).

### 3) Screen inventory — actual snapshot

> ⚠️ The counts below are a **snapshot taken when this prompt was written**. Re-verify with a grep before assigning work; waves since then have changed them.

Total `frm*` forms: **93**, of which **31** are reports. Screens calling all six helpers: **23**. Screens with no helper call: **40**.

**Group A — call all six (audit/review only, do not rebuild):**

```
Accounting/frmAccounts · Accounting/frmJournalEntries · CashBoxes/frmCashBoxMaster ·
Categories/frmCategoryMaster · CustomerReceipts/frmListCustomerReceipts · Customers/frmCustomerMaster ·
Employees/frmEmployeeMaster · Expenses/frmListExpenses · Inventory/frmListInventoryAdjustments ·
Inventory/frmListInventoryBatches · Inventory/frmListInventoryTransactions · Payments/frmListPayments ·
Payments/frmListSupplierPayments · POS/frmPOS · Products/frmProductMaster · Products/frmProductUnitMaster ·
Purchases/frmListPurchases · Returns/frmListReturns · Sales/frmListSales · Suppliers/frmSupplierMaster ·
Units/frmUnitMaster · Users/frmUserMaster · Warehouses/frmWarehouseMaster
```

> In this group verify: `ContextMenuStrip` (missing in most) · `btnToggleFilters` **declared in the designer** · `pnlSummary` · designer colour parity · `LiveTafqeetHelper` on money screens.

**Group B — partial navigation (4 of 6) — needs real completion:**

```
CashBoxes/frmCashBoxTransactions · CustomerReceipts/frmAddUpdateCustomerReceipt ·
Customers/frmCustomerSelection · Customers/frmCustomerStatement · Expenses/frmAddUpdateExpense ·
Payments/frmSupplierPaymentDialog · Products/frmProductSelection · Purchases/frmPurchaseInvoice ·
Returns/frmPurchaseReturn · Returns/frmSalesReturn · Sales/frmAddUpdateSalesInvoice ·
Suppliers/frmSupplierSelection · Users/frmBackup · Users/frmPasswordChange
```

> Most common gaps: `WireButtonDisabledVisual` + `WireGlobalShortcuts` + `FilterPanelHelper` + `pnlSummary`.

**Group C — no helper calls (build to the standard from scratch):**

```
Inventory/frmInventory · Inventory/frmInventoryList · Inventory/frmInventoryAddProduct ·
Inventory/frmAddUpdateInventoryAdjustment · Inventory/frmInventoryFifo · Inventory/frmInventoryBarcode ·
Inventory/frmOpeningStock · Inventory/frmStocktakingNew · Inventory/frmWarehouseTransferNew ·
Purchases/frmPurchaseItem · Purchases/frmPurchaseItemInfo · Purchases/frmPurchaseDiscount ·
Purchases/frmPurchaseCurrency · Purchases/frmPurchaseOtherDetails · Purchases/frmSupplierStatement ·
Purchases/frmPurchaseInvoicePrint · Inventory/frmStocktakingPrint · Inventory/frmStocktakingPrintAdjustment ·
Inventory/frmStocktakingPrintCard · Inventory/frmWarehouseTransferPrint · Products/frmImportProductsExcel ·
Tools/frmCalculator · Dashboard/Forms/frmDashboard · frmLogin · frmMain
```

### 4) Exceptions (never bend the standard without a reason)

| Type | Exception | Reference |
|---|---|---|
| 31 reports (`Reports/Forms/**`) | managed exclusively by `ReportUiStandardizer.ApplyForm(this)` + `ReportNavigationHelper.Attach(this, _serviceProvider)` + Excel export (`IExportableReport`); **exempt** from the six helpers and from `gbFilter` | RULE-N73/N121 |
| Print/preview screens (`frm*Print*` · `frmStocktakingPrint*`) | read-only; exempt from form filling and navigation — but still: green theme + RTL + Tahoma + a close button visible above any docked bar (RULE-N125) | §2.20 P3 |
| Pickers (`frmProductSelection` · `frmCustomerSelection` · `frmSupplierSelection`) | read-only: `pnlSummary`/`gbFilter` not mandatory, but grid keyboard navigation + `Enter` to select + `Esc` to cancel + theme | §2.20 P2 |
| Dialogs (`frmPurchaseItem` · `frmPurchaseDiscount` · `frmPurchaseCurrency` · `frmPurchaseOtherDetails` · `frmSupplierPaymentDialog` · `frmPasswordChange` · `frmBackup`) | theme + navigation + Arabic validation; no `gbFilter`/`pnlSummary` | — |
| `frmLogin` · `frmMain` · `frmDashboard` · `frmCalculator` | not treated as data-entry screens; theme + RTL + Tahoma only (the dashboard stays `ctrlDashboard` — RULE-N64…N69) | §2.23 |

### 5) Explicit prohibitions

- ❌ Do not edit `Program.cs` · `appsettings.json` · `*.csproj` · `SalesSystem.slnx` · `AGENTS.md` · `SalesSystem.Tests/**` — **report the need only** (RULE-N114/N115).
- ❌ No new NuGet packages, and no new interfaces/factories/brokers (RULE-N113).
- ❌ Do not register new services and do not touch `Services/Direct`/`Services/Mock`/`Services/Interfaces`.
- ❌ Do not delete a screen or a file — propose the deletion to the manager.
- ❌ No `dotnet test` and no running the app (RULE-N117). Allowed: `dotnet build` only.
- ❌ Never write numeric colours inside a screen — constants of `GreenThemeHelper` only (RULE-N54).
- ❌ Never duplicate behaviour that already lives in a helper (RULE-N98/N112).

### 6) Acceptance and delivery gate

**A. Build (mandatory):**

```powershell
dotnet build SalesSystem\SalesSystem.slnx
```

Zero errors. Any new warning must be listed.

**B. Delivery report per screen — table:**

| Screen | Blocks added/reordered | KeyboardHelper calls (6/6) | gbFilter | pnlSummary | Designer colours match | ContextMenu | Rules verified |
|---|---|---|---|---|---|---|---|

**C. Manual test checklist (RULE-N116) — table:**

| Screen | Step | Expected result |
|---|---|---|
| … | `F2`, then fill the fields with `Enter` | Mode header = "Add … new" and the buttons are correct |
| … | Select a row in the grid | Mode = "Edit …: <name>", "Delete/Edit" enabled |
| … | Type in `txtFilterValue` | Instant filtering + `pnlSummary` refreshed |
| … | `F9` / the "hide filters" button | Box collapses to height 34 and the state is remembered after reopening |
| … | Deactivate a record, tick `chkShowInactive`, click "Reactivate" | Success + row selected + Arabic message |
| … | `Esc` inside `gbFilter` | All filters cleared |

**D. Rejection:** any screen that does not satisfy §2.21, §2.27 and §2.33 is incomplete (RULE-N55/N99) — do not deliver it.

### 7) Execution batches (edit this list to contain only what is assigned to you)

| Batch | Screens | Note |
|---|---|---|
| 1 | `frmAddUpdateSalesInvoice` · `frmPurchaseInvoice` · `frmSalesReturn` · `frmPurchaseReturn` | daily documents — highest priority |
| 2 | `frmAddUpdateExpense` · `frmAddUpdateCustomerReceipt` · `frmCashBoxTransactions` · `frmSupplierPaymentDialog` | money documents + tafqeet |
| 3 | `frmCustomerStatement` · `frmSupplierStatement` · `frmInventory*` · `frmOpeningStock` · `frmStocktakingNew` · `frmWarehouseTransferNew` | inventory |
| 4 | `frmProductSelection` · `frmCustomerSelection` · `frmSupplierSelection` · `frmBackup` · `frmPasswordChange` · `frmImportProductsExcel` · `frmPurchaseItem*` · `frmPurchaseDiscount` · `frmPurchaseCurrency` · `frmPurchaseOtherDetails` | pickers/dialogs |
| 5 | Audit of Group A (review only) | no rebuild |

### 8) Sensitive designer details (WinForms pitfalls)

- Add `Controls.Add(x)` for every declared control — a declared control that is never added is a bug (RULE-N130).
- Never reorder `Controls` for a docked element — it flips the docking and covers the bottom bar (RULE-N125).
- When moving a control between containers: reset `Anchor` first, then `Controls.Add` (RULE-N126).
- `btnToggleFilters` **is declared in `*.Designer.cs`** with `Dock = Left` inside `pnlFilters`; the helper adopts it and never creates it (RULE-N145).
- Containers that inherited their parent's background (Ambient) or were coloured by the designer with a custom colour **are not repainted** at runtime (RULE-N51a/N139).
- `.Designer.cs` files contain no reference to `Helpers/` at all (RULE-N128).
- The path `Dashboard/Forms` is the correct one — fixed by a pure rename with no `namespace`/DI change (it used to be `Froms` — RULE-N124, closed).

### 9) Working notes

- Files are UTF-8: keep the Arabic intact and never produce mojibake. After every edit, re-read the file to verify.
- One screen per commit, and the commit message: `ui(<module>): <screen> — align with the frmProductMaster standard`.
- Do not work on more than one screen in the same file in one session without an explicit request.
- If you find a contradiction in `AGENTS.md` or a deviation in the reference screen — report it, do not decide on your own.

## Prompt end