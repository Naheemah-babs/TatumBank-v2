
using TatumWelcome2.BLL;
using TatumWelcome2.DTOs;
using TatumWelcome2.Models;

namespace TatumWelcome2.Controllers;

public static class BillPaymentEndpoints
{
    public static void MapBillPaymentEndpoints(
        this WebApplication app)
    {
        // Return available network providers.
        app.MapGet("/bills/networks", () =>
        {
            return Results.Ok(DataPlans.Networks);
        });

        // Return available data plans.
        app.MapGet("/bills/plans", () =>
        {
            return Results.Ok(DataPlans.DataPlanItems);
        });

        // Process airtime or data purchase requests.
        app.MapPost("/bills/pay",
    (BillPaymentRequestDto request, BillPaymentService service) =>
    {
        BillPaymentResponseDto response =
            service.ProcessPayment(request);

        return Results.Ok(response);
    });
    }
}
