# UI Screens Reference — WinForms Desktop

## Architectural Standard
All screens MUST follow the WinForms N-Layer patterns as strictly defined in `AGENTS.md`. No WPF, XAML, or MVVM patterns are allowed.

## Core Screen Conventions

### Data Forms (List/Grids)
- **Naming**: `frmList*` (e.g., `frmListProducts`, `frmListCustomers`, `frmListSales`)
- **Structure**:
  1. Header Label (`lblMode`, Dock=Top)
  2. Filter Bar (ComboBox `cbFilterBy`, TextBox `txtFilterValue`)
  3. DataGridView (`dgvData`)
     - `AllowUserToAddRows = false`
     - `ReadOnly = true`
     - `SelectionMode = FullRowSelect`
     - ContextMenuStrip for row operations (Edit, Deactivate)
  4. Footer Buttons (Add, Close)

### Editor Forms (Create/Update)
- **Naming**: `frmAddUpdate*` (e.g., `frmAddUpdateProduct`, `frmAddUpdateCustomer`)
- **Structure**:
  1. Header Label (`lblMode`, Dock=Top, shows "Add" or "Edit")
  2. Input Fields (TextBox, ComboBox, CheckBox) with standard validation logic
  3. Save & Cancel Buttons
- **Behavior**:
  - Modal display via `ShowDialog()`
  - Uses `try/catch` and `MessageBox.Show` for user feedback (Arabic only)

## Multi-Currency Display Patterns
All financial screens must show currency alongside amounts:
- **Invoice Line**: `بطاطس عمان | 10 حبة | 500 YER | 5,000 YER`
- **Invoice Total**: `الإجمالي: 15,000 YER (≈ $21.43 USD)`

## Screen Flow (UI First)
1. **Login**: Authenticates and stores JWT in memory.
2. **Dashboard / Main Form**: Houses the Sidebar menu. Applies Role-Based Visibility (e.g., Admin sees Warehouses/Settings; Cashier sees Sales).
3. **Module Flow**: `frmMain` -> `frmList*` -> `frmAddUpdate*` (Modal).

*For specific UI styling, RightToLeft properties, fonts (Tahoma), and interaction rules, see `AGENTS.md`.*
