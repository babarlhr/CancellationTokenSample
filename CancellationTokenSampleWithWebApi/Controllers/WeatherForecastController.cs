using CancellationTokenSample.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CancellationTokenSample.Controllers;

[ApiController]
[Route("tasks")]
public class WeatherForecastController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger,
        ApplicationDbContext context)
    {
        // Constructor to initialize logger and database context.
        _logger = logger;
        _context = context;
    }


    [HttpGet("no-cancellation")]
    public async Task<ActionResult> Get()
    {
        // API endpoint with no cancellation token.
        // Performs a simple task that takes 30 seconds.
        await DoSomething(Random.Shared.Next(1, 1000));

        return Ok("DONE!!!");
    }


    [HttpGet("with-cancellation-source")]
    public async Task<ActionResult> GetWithCancellationSource()
    {
        // API endpoint with a cancellation token source.
        // Automatically cancels the task if it runs for more than 10 seconds.

        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));

        await DoSomething(Random.Shared.Next(1, 1000), cancellationTokenSource.Token);
        return Ok("DONE!!!");
    }

    /// <summary>
    /// This will abort the current request in a long running process from client side , browser refresh, browser close ..  and also will close the all depending processes
    /// long running dependence processes Api=> services => Repository code etc
    /// But if a process is running in detached mode that will not be cancel,  for example if current request run a long running query on SQL server even though request is canceled but query on SQL will be not aborted.
    /// here HttpContext.RequestAborted is cancellation token already built into HttpContext so cancellationTokenSource is not needed in method parameter.
    /// </summary>
    /// <returns></returns>
    [HttpGet("with-request-cancellation")]
    public async Task<ActionResult> GetWithCancellationRequest()
    {

        // API endpoint using the built-in cancellation token from HttpContext.
        // Cancels the task if the client closes the browser or refreshes the page.

        await DoSomething(Random.Shared.Next(1, 1000), HttpContext.RequestAborted);

        /*
         Api
            ->> Service
                ->> Repository
         */

        return Ok("DONE!!!");
    }

    /// <summary>
    ///Uncomment for not using cancellation token from HttpContext .Not recommended this approach is only for Demo
    ///if we use this approach then we have to add this "CancellationToken cancellationToken"  parameter to all our end pints which is annoying.
    ///we need one extra CancellationToken cancellationToken parameter for all APIs.
    /// </summary>
    /// <returns></returns>
    //[HttpGet("with-request-cancellation")]
    //public async Task<ActionResult> GetWithCancellationRequest(CancellationToken cancellationToken)
    //{

    //    // Cancels the task if the client closes the browser or refreshes the page.

    //    await DoSomething(Random.Shared.Next(1, 1000), cancellationToken);

    //    /*
    //     Api
    //        ->> Service
    //            ->> Repository
    //     */

    //    return Ok("DONE!!!");
    //}


    [HttpGet("with-ef-query-cancellation")]
    public async Task<ActionResult> GetWithCancellationQuert()
    {

        //waitfor delay '00:00:10' is used in SQL to instruct SQL server to run a query after delay .
        // API endpoint demonstrating cancellation of a long-running Entity Framework query.
        await _context.Database
            .ExecuteSqlRawAsync("waitfor delay '00:00:10'; select * from Test", HttpContext.RequestAborted); 

        return Ok("DONE!!!");
    }

    //Mock Up for Console output
    private async Task DoSomething(int number = 0, CancellationToken cancellationToken = default)
    {
        try
        {
            // Simulates a task that runs for 30 seconds, writing to the console.

            for (int i = 0; i < 30; i++)
            {
                await Task.Delay(1000, cancellationToken);
                Console.WriteLine($"*** Task is running: {number} ***");
            }
        }
        catch (Exception ex)
        {
            // Logs any errors that may occur during the task.
            _logger.LogError(ex.Message);
        }
    }
}