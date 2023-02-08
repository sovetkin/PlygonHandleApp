using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using PlygonHandle.Logic;
using PlygonHandle.Validation;

using control = System.Windows.Controls;

namespace PlygonHandle.ViewModels
{
    public class MainViewModel : ObservableValidator
    {
        #region Private Fields
        private string _listFirstPolygonPoints;
        private string _listSecomdPolygonPoints;
        private PointCollection _firstPolygonPoints;
        private PointCollection _secondPolygonPoints;
        private ICommand _exitCommand;
        private ICommand _setCanvasBoundaries;
        private ICommand _newPolygonRendered;

        private double _width;
        private double _height;
        private double _polygonArea; 
        #endregion

        #region Public Constructors
        public MainViewModel()
        {
            _firstPolygonPoints = new();
            _secondPolygonPoints = new();
        } 
        #endregion

        #region Public Command
        public ICommand ExitCommand => _exitCommand ??= new RelayCommand(OnExitCommand);
        public ICommand SetCanvasBoundaries => _setCanvasBoundaries ??= new RelayCommand<object>(OnSetCanvasBoundaries);

        public ICommand NewPolygonRendered => _newPolygonRendered ??= new RelayCommand<object>(OnNewPolygonRendered); 
        #endregion

        #region Public Properties
        public double PolygonArea
        {
            get => _polygonArea;
            set
            {
                SetProperty(ref _polygonArea, value);
            }
        }

        public PointCollection FirstPolygonPoints
        {
            get => _firstPolygonPoints;
            set
            {
                SetProperty(ref _firstPolygonPoints, value);
                OnPropertyChanged(nameof(FirstStartPoint));
                OnPropertyChanged(nameof(FirstPolygonRestPoints));
            }
        }

        public PointCollection SecondPolygonPoints
        {
            get => _secondPolygonPoints;
            set
            {
                SetProperty(ref _secondPolygonPoints, value);
                OnPropertyChanged(nameof(SecondStartPoint));
                OnPropertyChanged(nameof(SecondPolygonRestPoints));
            }
        }

        public Point FirstStartPoint => FirstPolygonPoints.Count == 0 ? new Point(-1, -1) : FirstPolygonPoints[0];
        public Point SecondStartPoint => SecondPolygonPoints.Count == 0 ? new Point(-1, -1) : SecondPolygonPoints[0];

        public PointCollection FirstPolygonRestPoints
        {
            get
            {
                Point[] points = FirstPolygonPoints?.ToArray();
                return points.Length == 0 ? default : new PointCollection(points[1..]);
            }
        }

        public PointCollection SecondPolygonRestPoints
        {
            get
            {
                Point[] points = SecondPolygonPoints.ToArray();
                return points.Length == 0 ? default : new PointCollection(points[1..]);
            }
        }

        [CustomValidation(typeof(MainViewModel), nameof(ValidateString))]
        public string ListFirstPolygonPoints
        {
            get => _listFirstPolygonPoints;
            set
            {
                SetProperty(ref _listFirstPolygonPoints, value, true);

                var errors = GetErrors(nameof(ListFirstPolygonPoints)).ToList();

                if (errors.Count == 0)
                    FirstPolygonPoints = PointCollection.Parse(value);
            }
        }

        [CustomValidation(typeof(MainViewModel), nameof(ValidateString))]
        public string ListSecondPolygonPoints
        {
            get => _listSecomdPolygonPoints;
            set
            {
                SetProperty(ref _listSecomdPolygonPoints, value, true);

                var errors = GetErrors(nameof(ListSecondPolygonPoints)).ToList();

                if (errors.Count == 0)
                    SecondPolygonPoints = PointCollection.Parse(value);
            }
        } 
        #endregion

        #region Public Methods
        public static ValidationResult ValidateString(string value, ValidationContext context)
        {
            var obj = (MainViewModel)context.ObjectInstance;

            return StringValidationRules.Validate(value, obj._width, obj._height);
        } 
        #endregion

        #region Private Methods
        private void OnNewPolygonRendered(object obj) => PolygonArea = PolygonHelper.GetPolygonArea((Path)obj);

        private void OnSetCanvasBoundaries(object obj)
        {
            var value = (control::Canvas)obj;

            _width = value.ActualWidth;
            _height = value.ActualHeight;
        }

        private void OnExitCommand() => Application.Current.Shutdown();
        #endregion
    }
}
