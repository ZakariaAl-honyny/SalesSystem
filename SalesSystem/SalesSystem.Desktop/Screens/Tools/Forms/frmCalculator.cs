using System.Globalization;

namespace SalesSystem.Desktop.Screens.Tools.Forms;

/// <summary>
/// آلة حاسبة بسيطة (واجهة تجريبية / Mock) — لا تتصل بأي خدمة خلفية.
/// عمليات: جمع، طرح، ضرب، قسمة، فاصلة عشرية، مسح، حذف آخر رقم.
/// </summary>
public partial class frmCalculator : Form
{
    private readonly IServiceProvider _serviceProvider;

    // ---- حالة الآلة الحاسبة (في الذاكرة فقط) ----
    private string _input = "0";              // الرقم الحالي المُدخل
    private decimal _accumulator;             // النتيجة المتراكمة
    private string? _pendingOperator;         // "+" | "−" | "×" | "÷"
    private bool _isNewEntry = true;          // الرقم التالي يبدأ إدخالاً جديداً
    private bool _isError;                    // حالة خطأ (القسمة على صفر ... إلخ)

    public frmCalculator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
    }

    // ---- أزرار الأرقام ----
    private void Digit_Click(object? sender, EventArgs e)
        => DigitPress(((Button)sender!).Text);

    private void DigitPress(string digit)
    {
        if (_isError) ResetAll();

        if (_isNewEntry)
        {
            _input = digit == "0" ? "0" : digit;
            _isNewEntry = false;
        }
        else
        {
            _input = _input == "0" ? digit : _input + digit;
        }

        UpdateDisplay();
    }

    // ---- الفاصلة العشرية ----
    private void Dot_Click(object? sender, EventArgs e)
    {
        if (_isError) ResetAll();

        if (_isNewEntry)
        {
            _input = "0.";
            _isNewEntry = false;
        }
        else if (!_input.Contains('.'))
        {
            _input += ".";
        }

        UpdateDisplay();
    }

    // ---- العمليات الحسابية (+ − × ÷) ----
    private void Operator_Click(object? sender, EventArgs e)
        => OperatorPress(((Button)sender!).Text);

    private void OperatorPress(string op)
    {
        if (_isError) return;

        if (!_isNewEntry && TryParseInput(out decimal current))
        {
            if (_pendingOperator != null)
            {
                if (!TryCompute(_accumulator, current, _pendingOperator, out decimal result))
                    return; // الخطأ ظهر وتمت إعادة الضبط

                _accumulator = result;
                _input = FormatNumber(result);
            }
            else
            {
                _accumulator = current;
            }
        }

        _pendingOperator = op;
        _isNewEntry = true;
        UpdateDisplay();
    }

    // ---- زر النتيجة (=) ----
    private void Equals_Click(object? sender, EventArgs e)
    {
        if (_isError || _pendingOperator == null || _isNewEntry) return;

        if (TryParseInput(out decimal current)
            && TryCompute(_accumulator, current, _pendingOperator, out decimal result))
        {
            _accumulator = result;
            _input = FormatNumber(result);
            _pendingOperator = null;
            _isNewEntry = true;
            UpdateDisplay();
        }
    }

    // ---- مسح (C) ----
    private void Clear_Click(object? sender, EventArgs e) => ResetAll();

    // ---- حذف آخر رقم (⌫) ----
    private void Backspace_Click(object? sender, EventArgs e)
    {
        if (_isError)
        {
            ResetAll();
            return;
        }

        if (_isNewEntry) return;

        _input = _input.Length > 1 ? _input[..^1] : "0";
        UpdateDisplay();
    }

    // ---- إغلاق ----
    private void btnClose_Click(object? sender, EventArgs e) => Close();

    // ---- دعم لوحة المفاتيح ----
    private void frmCalculator_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.D0:
            case Keys.NumPad0: DigitPress("0"); e.Handled = true; e.SuppressKeyPress = true; break;
            case Keys.D1:
            case Keys.NumPad1: DigitPress("1"); e.Handled = true; e.SuppressKeyPress = true; break;
            case Keys.D2:
            case Keys.NumPad2: DigitPress("2"); e.Handled = true; e.SuppressKeyPress = true; break;
            case Keys.D3:
            case Keys.NumPad3: DigitPress("3"); e.Handled = true; e.SuppressKeyPress = true; break;
            case Keys.D4:
            case Keys.NumPad4: DigitPress("4"); e.Handled = true; e.SuppressKeyPress = true; break;
            case Keys.D5:
            case Keys.NumPad5: DigitPress("5"); e.Handled = true; e.SuppressKeyPress = true; break;
            case Keys.D6:
            case Keys.NumPad6: DigitPress("6"); e.Handled = true; e.SuppressKeyPress = true; break;
            case Keys.D7:
            case Keys.NumPad7: DigitPress("7"); e.Handled = true; e.SuppressKeyPress = true; break;
            case Keys.D8:
            case Keys.NumPad8: DigitPress("8"); e.Handled = true; e.SuppressKeyPress = true; break;
            case Keys.D9:
            case Keys.NumPad9: DigitPress("9"); e.Handled = true; e.SuppressKeyPress = true; break;
            case Keys.OemPeriod:
            case Keys.Decimal: Dot_Click(null, EventArgs.Empty); e.Handled = true; e.SuppressKeyPress = true; break;
            case Keys.Add: OperatorPress("+"); e.Handled = true; e.SuppressKeyPress = true; break;
            case Keys.OemMinus:
            case Keys.Subtract: OperatorPress("−"); e.Handled = true; e.SuppressKeyPress = true; break;
            case Keys.Multiply: OperatorPress("×"); e.Handled = true; e.SuppressKeyPress = true; break;
            case Keys.Divide: OperatorPress("÷"); e.Handled = true; e.SuppressKeyPress = true; break;
            case Keys.Enter: Equals_Click(null, EventArgs.Empty); e.Handled = true; e.SuppressKeyPress = true; break;
            case Keys.Back: Backspace_Click(null, EventArgs.Empty); e.Handled = true; e.SuppressKeyPress = true; break;
            case Keys.Delete:
            case Keys.C when e.Modifiers == Keys.None: Clear_Click(null, EventArgs.Empty); e.Handled = true; e.SuppressKeyPress = true; break;
            case Keys.Escape: Close(); e.Handled = true; e.SuppressKeyPress = true; break;
        }
    }

    // ---- أدوات مساعدة ----
    private bool TryParseInput(out decimal value)
        => decimal.TryParse(_input, NumberStyles.Float, CultureInfo.InvariantCulture, out value);

    private bool TryCompute(decimal left, decimal right, string op, out decimal result)
    {
        result = 0m;

        try
        {
            switch (op)
            {
                case "+": result = left + right; return true;
                case "−": result = left - right; return true;
                case "×": result = left * right; return true;
                case "÷":
                    if (right == 0m)
                    {
                        ShowError();
                        return false;
                    }
                    result = left / right;
                    return true;
                default: return false;
            }
        }
        catch (OverflowException)
        {
            ShowError();
            return false;
        }
    }

    private static string FormatNumber(decimal value)
        => value.ToString("0.########################", CultureInfo.InvariantCulture);

    private void UpdateDisplay() => txtDisplay.Text = _input;

    private void ShowError()
    {
        _isError = true;
        _input = "0";
        _accumulator = 0m;
        _pendingOperator = null;
        _isNewEntry = true;
        txtDisplay.Text = "خطأ";
    }

    private void ResetAll()
    {
        _isError = false;
        _input = "0";
        _accumulator = 0m;
        _pendingOperator = null;
        _isNewEntry = true;
        txtDisplay.Text = "0";
    }
}