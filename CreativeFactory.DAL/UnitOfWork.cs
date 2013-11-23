using System;

namespace CreativeFactory.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Private Fields
        private readonly CreativeFactoryContext _context = new CreativeFactoryContext();

        private ArticleRepository _articleRepository;

        private ItemRepository _itemRepository;

        private RatingRepository _ratingRepository;

        private TagRepository _tagRepository;

        private UserRepository _userRepository;
        #endregion

        #region Properties
        public ArticleRepository ArticleRepository
        {
            get { return _articleRepository ?? (_articleRepository = new ArticleRepository(_context)); }
        }

        public ItemRepository ItemRepository
        {
            get { return _itemRepository ?? (_itemRepository = new ItemRepository(_context)); }
        }

        public RatingRepository RatingRepository
        {
            get { return _ratingRepository ?? (_ratingRepository = new RatingRepository(_context)); }
        }

        public TagRepository TagRepository
        {
            get { return _tagRepository ?? (_tagRepository = new TagRepository(_context)); }
        }

        public UserRepository UserRepository
        {
            get { return _userRepository ?? (_userRepository = new UserRepository(_context)); }
        }
        #endregion

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
