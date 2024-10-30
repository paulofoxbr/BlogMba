namespace Blog.Data.Dto
{
    public class ResponsePostAutor
    {
        public int TotalPages => (int)Math.Ceiling((double)TotalPosts / PostsPerPage);

        public int TotalPosts { get; set; }
        public int PostsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public List<PostAuthorDto> PostAutor { get; set; }

        public ResponsePostAutor(List<PostAuthorDto> postAutor, int totalPosts, int postsPerPage, int currentPage)
        {
            PostAutor = postAutor;
            TotalPosts = totalPosts;
            PostsPerPage = postsPerPage;
            CurrentPage = currentPage;
        }
    }
}