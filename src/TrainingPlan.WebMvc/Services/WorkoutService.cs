using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TrainingPlan.ApplicationCore.Entities;
using TrainingPlan.WebMvc.Infrastructure;
using TrainingPlan.WebMvc.ViewModels;

namespace TrainingPlan.WebMvc.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly IHttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly string _remoteServiceBaseUrl;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly IOptionsSnapshot<AppSettings> _settings;

        public WorkoutService(IMapper mapper, IOptionsSnapshot<AppSettings> settings, IHttpClient httpClient)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _remoteServiceBaseUrl =
                $"{_settings.Value.TrainingPlanApiUrl}{_settings.Value.TrainingPlanApiVersion}workouts";
        }

        public async Task CreateAsync(WorkoutViewModel workoutViewModel)
        {
            var uri = _remoteServiceBaseUrl;
            var response =
                await _httpClient.PostEntityAsync(uri, _mapper.Map<WorkoutViewModel, Workout>(workoutViewModel));

            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<WorkoutViewModel>> GetAllAsync()
        {
            var uri = _remoteServiceBaseUrl;
            var dataString = await _httpClient.GetStringAsync(uri);

            return _mapper.Map<IEnumerable<Workout>, IEnumerable<WorkoutViewModel>>(
                JsonConvert.DeserializeObject<List<Workout>>(dataString));
        }

        public async Task<WorkoutViewModel> GetByIdAsync(int id)
        {
            var uri = $"{_remoteServiceBaseUrl}/{id}";
            var dataString = await _httpClient.GetStringAsync(uri);

            return dataString != null
                ? _mapper.Map<Workout, WorkoutViewModel>(JsonConvert.DeserializeObject<Workout>(dataString))
                : null;
        }
    }
}