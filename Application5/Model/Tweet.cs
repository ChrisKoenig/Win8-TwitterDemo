using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Application5.Model
{
    public class Tweet : ObservableObject
    {
        private string _text;

        public string Text
        {
            get { return _text; }
            set { 
                if (_text == value)
                    return;
                _text = value;
                RaisePropertyChanged("Text");
            }
        }

        private string _poster;
        public string Poster
        {
            get { return _poster; }
            set
            {
                if (_poster == value)
                    return;
                _poster = value;
                RaisePropertyChanged("Poster");
            }
        }

        private string _image;
        public string Image
        {
            get { return _image; }
            set
            {
                if (_image == value)
                    return;
                _image = value;
                RaisePropertyChanged("Image");
            }
        }

        private string _link;
        public string Link
        {
            get { return _link; }
            set
            {
                if (_link == value)
                    return;
                _link = value;
                RaisePropertyChanged("Link");
            }
        }
    }
}
