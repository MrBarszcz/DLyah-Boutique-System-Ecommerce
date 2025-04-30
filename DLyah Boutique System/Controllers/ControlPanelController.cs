using DLyah_Boutique_System.Models;
using DLyah_Boutique_System.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DLyah_Boutique_System.Controllers;

public class ControlPanelController : Controller {
    private readonly ILogger<ControlPanelController> _logger;

    public ControlPanelController( ILogger<ControlPanelController> logger ) {
        _logger = logger;
    }

    // GET
    public IActionResult Index() { // briefly list the requests
        return View();
    }

    
    // POST
}