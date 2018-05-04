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
        public Movie CreateMovie(Movie film)
        {
            throw new NotImplementedException();
        }

        public bool DeleteMovie(Movie film)
        {
            throw new NotImplementedException();
        }

        public List<Movie> getAllMovies()
        {
            DataModelContainer ctx = new DataModelContainer();
            return ctx.MovieSet.ToList();
        }

        public Movie GetMovie(int fid)
        {
            DataModelContainer ctx = new DataModelContainer();
            return ctx.MovieSet
                .Where(u => u.Id == fid)
                .Select(u => u)
                .FirstOrDefault();
        }

        public Movie UpdateMovie(Movie movie)
        {
            throw new NotImplementedException();
        }
    }
}
