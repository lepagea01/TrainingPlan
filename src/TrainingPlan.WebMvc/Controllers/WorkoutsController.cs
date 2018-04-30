using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrainingPlan.WebMvc.Services;
using TrainingPlan.WebMvc.ViewModels;

namespace TrainingPlan.WebMvc.Controllers
{
    public class WorkoutsController : BaseController
    {
        private readonly IWorkoutService _workoutService;

        public WorkoutsController(IWorkoutService workoutService)
        {
            _workoutService = workoutService ?? throw new ArgumentNullException(nameof(workoutService));
        }

        // GET: Workouts
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = await _workoutService.GetAllAsync();

            return View(viewModel);
        }

        // GET: Workouts/Details/5
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Details([FromRoute] int id)
        {
            var viewModel = await _workoutService.GetByIdAsync(id);

            if (viewModel == null) return NotFound();

            return View(viewModel);
        }

        // GET: Workouts/Create
        [HttpGet("[action]")]
#pragma warning disable 1998
        public async Task<IActionResult> Create()
#pragma warning restore 1998
        {
            return View();
        }

        // POST: Workouts/Create
        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromForm] WorkoutViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            await _workoutService.CreateAsync(viewModel);
            return RedirectToAction(nameof(Index));
        }

        // GET: Workouts/Edit/5
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var viewModel = await _workoutService.GetByIdAsync(id);

            if (viewModel == null) return NotFound();

            return View(viewModel);            
        }
    }
}