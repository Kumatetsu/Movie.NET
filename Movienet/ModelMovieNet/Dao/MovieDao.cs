using ModelMovieNet.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelMovieNet.Dao
{
    class MovieDao : IMovieDao
    {
        public Movie CreateMovie(Movie movie)
        {
            DataModelContainer ctx = DataModelContainer.GetDb();
            Console.WriteLine("movie in create: " + movie.Title);
            ctx.MovieSet.Add(movie);
            ctx.SaveChanges();
            return movie;
        }

        public bool DeleteMovie(Movie movie)
        {
            DataModelContainer ctx = DataModelContainer.GetDb();
            Movie toDelete = ctx.MovieSet.Where(m => m.Id == movie.Id).FirstOrDefault();
            ctx.MovieSet.Remove(toDelete);
            ctx.SaveChanges();
            return true;
        }
        
        public List<Movie> SearchMovies(string query)
        {
            Console.WriteLine("SearchMovies");
            DataModelContainer ctx = new DataModelContainer();
            List<string> query_words = query.Split(' ', ',').ToList();
            List<Movie> results = ctx.MovieSet.Where(m => query_words.Any(q => m.Title.Contains(q) || m.Abstract.Contains(q) || m.Type.Contains(q))).ToList();
            return results;
           
        }

        public List<Movie> getAllMovies()
        {
            DataModelContainer ctx = DataModelContainer.GetDb();
            return ctx.MovieSet.ToList();
        }

        public Movie GetMovie(int mid)
        {
            DataModelContainer ctx = DataModelContainer.GetDb();
            return ctx.MovieSet.Where(m => m.Id == mid).FirstOrDefault();
        }

        public Movie UpdateMovie(Movie movie)
        {
            Console.WriteLine("Movie passed to update: " + movie.ToString());
            DataModelContainer ctx = DataModelContainer.GetDb();
            Movie toUpdate = ctx.MovieSet.Where(m => m.Id == movie.Id).FirstOrDefault();
            Console.WriteLine("In UpdateMovie, return of update method: " + toUpdate.ToString());
            toUpdate.Title = movie.Title;
            toUpdate.Type = movie.Type;
            toUpdate.Abstract = movie.Abstract;
            toUpdate.Comments = movie.Comments;
            if (toUpdate.Equals(movie))
            {
                Console.WriteLine("Update ok");
                ctx.SaveChanges();
                return toUpdate;
            }
            else
            {
                throw new Exception("Update movie failed");
            }
        }
    }
}
