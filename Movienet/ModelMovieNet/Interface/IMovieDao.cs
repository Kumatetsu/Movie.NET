using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelMovieNet.Interface
{
    public interface IMovieDao
    {
        Movie CreateMovie(Movie movie);
        Movie UpdateMovie(Movie movie);
        List<Movie> SearchMovies(string query);
        Boolean DeleteMovie(Movie movie);
        Movie GetMovie(int fid);
        List<Movie> getAllMovies();
    }
}
