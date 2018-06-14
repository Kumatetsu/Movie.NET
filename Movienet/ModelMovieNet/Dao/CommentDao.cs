using ModelMovieNet;
using ModelMovieNet.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelMovieNet.Dao
{
    class CommentDao : ICommentDao
    {
        public Comment CreateComment(Comment comment)
        {
            DataModelContainer ctx = DataModelContainer.GetDb();
            Console.WriteLine("comment in create: " + comment.ToString());
            ctx.MovieSet.Attach(comment.Movie);
            ctx.UserSet.Attach(comment.User);
            ctx.CommentSet.Add(comment);
            ctx.SaveChanges();
            return comment;
        }

        public bool DeleteComment(Comment comment)
        {
            DataModelContainer ctx = DataModelContainer.GetDb();
            Comment toDelete = ctx.CommentSet.Where(c => c.Id == comment.Id).FirstOrDefault();
            ctx.CommentSet.Remove(toDelete);
            ctx.SaveChanges();
            return true;
        }

        public List<Comment> getAllComments()
        {
            DataModelContainer ctx = DataModelContainer.GetDb();
            return ctx.CommentSet.ToList();
        }

        public Comment GetComment(int cid)
        {
            DataModelContainer ctx = DataModelContainer.GetDb();
            return ctx.CommentSet.Where(c => c.Id == cid).FirstOrDefault();
        }

        public Comment UpdateComment(Comment comment)
        {
            Console.WriteLine("Comment passed to update: " + comment.ToString());
            DataModelContainer ctx = DataModelContainer.GetDb();
            Comment toUpdate = ctx.CommentSet.Where(c => c.Id == comment.Id).FirstOrDefault();
            Console.WriteLine("In UpdateComment, return of update method: " + toUpdate.ToString());
            toUpdate.Message = comment.Message;
            toUpdate.Movie = comment.Movie;
            toUpdate.Note = comment.Note;
            toUpdate.User = comment.User;
            if (toUpdate.Equals(comment))
            {
                Console.WriteLine("Update ok");
                ctx.SaveChanges();
                return toUpdate;
            }
            else
            {
                throw new Exception("Update comment failed");
            }
        }
    }
}
