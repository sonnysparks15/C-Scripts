using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Movie_Library.Data;
using Movie_Library.Models;
namespace Movie_Library.Controllers;
/* //////////////////////////////////////////////////////////////////////// */


/* //////////////////////////////////////////////////////////////////////// */



public class HomeController : Controller
{

    private readonly ILogger<HomeController> _logger;
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    
/* /////////////////////////////// */
    public IActionResult Index()
    {
        return View();
    }
    
/* /////////////////////////////// */
    public IActionResult Privacy()
    {
        return View();
    } 
    
/* /////////////////////////////// */
    public IActionResult Movie()
    {
        List<MovieMod> movies = new List<MovieMod>();
        MovieDAO movieDAO = new MovieDAO();

        movies = movieDAO.ReadIn();
        
        return View("Movie", movies);
    }
    
/* /////////////////////////////// */
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
/* /////////////////////////////// */
    public IActionResult Create(MovieMod movieMod)
    {
        return View("MovieModForm");
    }
    public IActionResult CreateProcess(MovieMod movieMod)
    {
        MovieDAO movieDao = new MovieDAO();
        movieDao.Create(movieMod);
        
        return View("Details", movieMod);
    }

/* /////////////////////////////// */
    public IActionResult Details(string filmTitle)
    {
        MovieDAO movieDao = new MovieDAO();
        MovieMod movieMod = movieDao.Details(filmTitle);
        return View("Details", movieMod);
    }
    
/* /////////////////////////////// */
    public IActionResult Delete(string filmTitle)
    {
        MovieDAO movieDAO = new MovieDAO();
        movieDAO.Delete(filmTitle);
        
        List<MovieMod> movies = movieDAO.ReadIn();

        return View("Movie", movies);
    }
/* /////////////////////////////// */
}
