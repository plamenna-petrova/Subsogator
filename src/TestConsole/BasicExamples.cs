using Data.DataAccess;
using Data.DataModels.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TestConsole
{
    public static class BasicExamples
    {
        public class GenreDetailsModel
        {
            public string Name { get; set; }

            public DateTime CreatedOn { get; set; }
        }
        public static void Run()
        {
            IList<Genre> genresToAdd = new List<Genre>()
            {
                new Genre()
                {
                    Name = "Romance"
                },
                new Genre()
                {
                    Name = "Coming of Age"
                },
                new Genre()
                {
                    Name = "Horror"
                }
            };

            try
            {
                using (var applicationDbContext = new ApplicationDbContext())
                {
                    Genre newGenre = new Genre()
                    {
                        Name = "Sci-Fi"
                    };

                    //if (!applicationDbContext.Genres.Any(g => g.Name == newGenre.Name))
                    //{
                    //    applicationDbContext.Add(newGenre);
                    //}

                    if (applicationDbContext.Genres.Any(g => g.Name == newGenre.Name))
                    {
                        var sciFiGenre = applicationDbContext.Genres
                                .Where(g => g.Name == newGenre.Name)
                                .SingleOrDefault();
                        applicationDbContext.Remove(sciFiGenre);
                    }

                    //applicationDbContext.Genres.RemoveRange(applicationDbContext.Genres.Where(g => g.Name == "Romance" || g.Name == "Horror" || g.Name == "Coming of Age"));

                    genresToAdd.ToList().ForEach(x =>
                    {
                        Expression<Func<Genre, bool>> genreRemovalPedicate = g => g.Name == x.Name;
                        if (applicationDbContext.Genres.Any(genreRemovalPedicate))
                        {
                            var genreToRemove = applicationDbContext.Genres
                                .Where(genreRemovalPedicate)
                                .SingleOrDefault();
                            applicationDbContext.Genres.Remove(genreToRemove);
                        }
                    });

                    applicationDbContext.SaveChanges();

                    //genresToAdd.ToList().ForEach(x =>
                    //{
                    //    if (!applicationDbContext.Genres.Any(g => g.Name == x.Name))
                    //    {
                    //        applicationDbContext.Add(x);
                    //    }
                    //});

                    //applicationDbContext.SaveChanges();

                    var genresStartingWithA = applicationDbContext.Genres
                            .Where(g => g.Name.ToLower().StartsWith("a"))
                            // .Select(g => g) - grab the whole entity object
                            .OrderByDescending(g => g.Name)
                            .Select(g => g.Name.Substring(0, 3).ToUpper())
                            .ToList();

                    foreach (var abbreviatedGenre in genresStartingWithA)
                    {
                        Console.WriteLine($"Genre: {abbreviatedGenre}");
                    }

                    Expression<Func<Genre, bool>> exampleGenresPredicate = g => 
                        g.Name.ToLower().StartsWith("a");

                    Expression<Func<Genre, GenreDetailsModel>> exampleGenresSelector =
                        g => new GenreDetailsModel()
                        {
                            Name = g.Name,
                            CreatedOn = g.CreatedOn
                        };

                    var singleGenreDetails = applicationDbContext.Genres
                        // .Where( g => g.Name.ToLower().StartsWith("a"))
                        .Where(exampleGenresPredicate)
                        //.Select(g => new GenreDetailsModel()
                        //  {
                        //    Name = g.Name,
                        //    CreatedOn = g.CreatedOn
                        //  })
                        .Select(exampleGenresSelector)
                        .FirstOrDefault();

                    Type genreDetailsType = singleGenreDetails.GetType();

                    foreach (var genreDetailsProperty in genreDetailsType.GetProperties())
                    {
                        Console.Write($"Property: {genreDetailsProperty.Name} " +
                        $"Value: {genreDetailsProperty.GetValue(singleGenreDetails, null)}");
                        Console.WriteLine();
                    }

                    var topFiveGenreResults = applicationDbContext.Genres
                        .Take(5)
                        .OrderBy(g => g.Name)
                        .AsNoTracking()
                        .ToList();

                    IDictionary<string, string> topGenresDictionary = new Dictionary<string, string>();

                    topFiveGenreResults.ForEach(x =>
                    {
                        //Console.WriteLine("Id: " + x.Id + " " + "Name: " + x.Name);
                        topGenresDictionary.Add(x.Id, x.Name);
                    });

                    foreach (KeyValuePair<string, string> topGenresKeyValuePair in topGenresDictionary)
                    {
                        Console.WriteLine("Key (Id) : {0}, Value (Name) : {1}",
                           topGenresKeyValuePair.Key, topGenresKeyValuePair.Value);
                    }

                    //var mysteryGenre = applicationDbContext.Genres.Find("33bc384");
                    //mysteryGenre.Name = "Mystery";
                    //applicationDbContext.Genres.Update(mysteryGenre);

                    applicationDbContext.SaveChanges();

                    var allCountriesByName = applicationDbContext.Countries
                        .Skip(3)
                        .OrderBy(c => c.CreatedOn)
                        // anonymous types
                        .Select(c => new
                        {
                            CountryName = c.Name,
                            CountryCreatedOn = c.CreatedOn
                        })
                        .AsNoTracking()
                        .ToList();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
