using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.BookStore.Books
{
    public static class DefaultUploadImage
    {
        public const string UploadImageBook = "/ImageBooks/";
    }
    public enum BookType
    {
        Undefined,
        Adventure,
        Biography,
        Dystopia,
        Fantastic,
        Horror,
        Science
    }
}
