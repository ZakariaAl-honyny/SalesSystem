using System;
using System.Windows.Forms;
using SalesSystem.Desktop.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace SalesSystem.Desktop.Screens.Products.Forms;

public partial class frmProductMaster : Form
{
    private readonly IServiceProvider _serviceProvider;
    private IProductApiService ProductService => _serviceProvider.GetRequiredService<IProductApiService>();

    public frmProductMaster(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
    }
}
