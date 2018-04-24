using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Interface
{
    public interface IFilmDao
    {
        Film CreateFilm(Film film);
        Film UpdateFilm(Film film);
        Boolean DeleteFilm(Film film);
        Film GetFilm(int fid);
        List<Film> getAllFilms();
    }
}
