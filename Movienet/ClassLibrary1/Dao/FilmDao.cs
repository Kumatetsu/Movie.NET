using ClassLibrary1.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Dao
{
    class FilmDao : IFilmDao
    {
        public Film CreateFilm(Film film)
        {
            DataModelContainer ctx = new DataModelContainer();
            ctx.Films.Add(film);
            ctx.SaveChanges();
            return film;
        }

        public bool DeleteFilm(Film film)
        {
            DataModelContainer ctx = new DataModelContainer();
            ctx.Films.Remove(film);
            ctx.SaveChanges();
            return true;
        }

        public List<Film> getAllFilms()
        {
            DataModelContainer ctx = new DataModelContainer();
            List<Film> films = ctx.Films.ToList();

            return films;
        }

        public Film GetFilm(int fid)
        {
            DataModelContainer ctx = new DataModelContainer();
            Film query = ctx.Films.Where(f => f.Id == fid).FirstOrDefault();

            return query;
        }

        public Film UpdateFilm(Film film)
        {
            DataModelContainer ctx = new DataModelContainer();
            Film updated = ctx.Films.Where(f => f.Id == film.Id).FirstOrDefault();
            updated = film;
            ctx.SaveChanges();
            return updated;
        }
    }
}
