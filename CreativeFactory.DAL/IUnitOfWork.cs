using System;

namespace CreativeFactory.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        ArticleRepository ArticleRepository { get; }

        ItemRepository ItemRepository { get; }

        RatingRepository RatingRepository { get; }

        TagRepository TagRepository { get; }

        UserRepository UserRepository { get; }

        void Save();
    }
}
