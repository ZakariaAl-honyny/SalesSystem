using System.Collections.Generic;

namespace SalesSystem.Business.Helpers;

/// <summary>
/// Allocates additional charges (transport, customs, etc.) proportionally across invoice line items.
/// Implements landed cost distribution based on line total weighting.
/// </summary>
public static class AdditionalChargeAllocator
{
    /// <summary>
    /// Allocates the total additional charge across items proportionally by line total.
    /// </summary>
    /// <param name="items">List of allocation lines with Index, LineTotal, Quantity, UnitCost.</param>
    /// <param name="totalAdditionalCharge">Total additional charge amount to distribute.</param>
    /// <param name="invoiceSubTotal">Subtotal of the invoice (sum of all LineTotals).</param>
    /// <returns>Dictionary mapping item index to landed unit cost.</returns>
    public static Dictionary<int, decimal> Allocate(
        IReadOnlyList<AllocationLine> items,
        decimal totalAdditionalCharge,
        decimal invoiceSubTotal)
    {
        var result = new Dictionary<int, decimal>();

        if (items == null || items.Count == 0 || invoiceSubTotal <= 0)
            return result;

        foreach (var item in items)
        {
            // Proportional share of additional charges based on line total weight
            var proportionalShare = item.Quantity > 0
                ? (item.LineTotal / invoiceSubTotal) * totalAdditionalCharge
                : 0;

            // Landed unit cost = original unit cost + (proportional share / quantity)
            var landedUnitCost = item.Quantity > 0
                ? item.UnitCost + (proportionalShare / item.Quantity)
                : item.UnitCost;

            result[item.Index] = landedUnitCost;
        }

        return result;
    }
}

/// <summary>
/// Represents a single line item for additional charge allocation.
/// </summary>
public class AllocationLine
{
    public int Index { get; set; }
    public decimal LineTotal { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitCost { get; set; }
}
