using HassadFood.ViewModels;
using HassadFood.WebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HassadFood.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ApprovalScreen : ContentPage
    {
        ApprovalModel viewModel;
        public ApprovalScreen(GetWorkList selected_type, string notification_type)
        {

            InitializeComponent();

            FlowDirection = Resx.AppResources.Culture.Parent.Name.ToString().Equals("ar") ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

            BindingContext = viewModel = new ApprovalModel(this, selected_type);


            if (notification_type.Equals("Open"))
            {
                switch (viewModel.SelectedNotification.action)
                {
                    case "N":
                        ApproverViewSubmit.IsVisible = false;
                        ApproverView.IsVisible = false;
                        NotificationView1.IsVisible = true;
                        NotificationView2.IsVisible = true;
                        NotificationView3.IsVisible = true;
                        // NotificationView4.IsVisible = true;
                        break;
                    case "P":
                        ApproverViewSubmit.IsVisible = true;
                        ApproverView.IsVisible = false;
                        NotificationView1.IsVisible = false;
                        NotificationView2.IsVisible = false;
                        NotificationView3.IsVisible = false;
                        //NotificationView4.IsVisible = false;
                        break;

                    case "R":
                        ApproverViewSubmit.IsVisible = false;
                        ApproverView.IsVisible = true;
                        NotificationView1.IsVisible = false;
                        NotificationView2.IsVisible = false;
                        NotificationView3.IsVisible = false;
                        // NotificationView4.IsVisible = false;
                        break;

                    default:
                        ApproverViewSubmit.IsVisible = false;
                        ApproverView.IsVisible = true;
                        NotificationView1.IsVisible = false;
                        NotificationView2.IsVisible = false;
                        NotificationView3.IsVisible = false;
                        // NotificationView4.IsVisible = false;
                        break;
                }

            }
            else if (notification_type.Equals("Closed"))
            {
                ApproverViewSubmit.IsVisible = false;
                ApproverView.IsVisible = true;
                NotificationView1.IsVisible = false;
                NotificationView2.IsVisible = false;
                NotificationView3.IsVisible = false;
            }
            else
            {

                switch (viewModel.SelectedNotification.action)
                {
                    case "N":
                        ApproverViewSubmit.IsVisible = false;
                        ApproverView.IsVisible = false;
                        NotificationView1.IsVisible = true;
                        NotificationView2.IsVisible = true;
                        NotificationView3.IsVisible = true;
                        // NotificationView4.IsVisible = true;
                        break;
                    case "P":
                        ApproverViewSubmit.IsVisible = true;
                        ApproverView.IsVisible = false;
                        NotificationView1.IsVisible = false;
                        NotificationView2.IsVisible = false;
                        NotificationView3.IsVisible = false;
                        //NotificationView4.IsVisible = false;
                        break;

                    case "R":
                        ApproverViewSubmit.IsVisible = false;
                        ApproverView.IsVisible = true;
                        NotificationView1.IsVisible = false;
                        NotificationView2.IsVisible = false;
                        NotificationView3.IsVisible = false;
                        // NotificationView4.IsVisible = false;
                        break;

                    case "Y":
                        ApproverViewSubmit.IsVisible = true;
                        ApproverView.IsVisible = false;
                        NotificationView1.IsVisible = false;
                        NotificationView2.IsVisible = false;
                        NotificationView3.IsVisible = false;
                        // NotificationView4.IsVisible = false;
                        break;

                    default:
                        ApproverViewSubmit.IsVisible = false;
                        ApproverView.IsVisible = true;
                        NotificationView1.IsVisible = false;
                        NotificationView2.IsVisible = false;
                        NotificationView3.IsVisible = false;
                        // NotificationView4.IsVisible = false;
                        break;
                }

            }




        }



        public void AddStackActionLine()
        {
            StackLayout stack = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("#D3D3D3"), HeightRequest = 0.5, Margin = new Thickness(10, 0, 10, 0), VerticalOptions = LayoutOptions.Start };
            DetailsStack.Children.Add(stack);
        }

        public void AddStackActionSeparatorLine()
        {
            StackLayout stack = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("#D3D3D3"), HeightRequest = 0.5, Margin = new Thickness(70, 0, 70, 0), VerticalOptions = LayoutOptions.Start };
            DetailsStack.Children.Add(stack);
        }

        public void AddStackAction()
        {
            StackLayout stack = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
            Label label2 = new Label { Text = "Action History", FontSize = 18, TextColor = Color.Black, HorizontalOptions = LayoutOptions.FillAndExpand, LineBreakMode = LineBreakMode.NoWrap };
          
            stack.Children.Add(label2);
            DetailsStack.Children.Add(stack);
        }

        public void AddStackMoreInfo()
        {
            StackLayout stack = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
            Label label2 = new Label { Text = "More Information", FontSize = 18, TextColor = Color.Black, HorizontalOptions = LayoutOptions.FillAndExpand, LineBreakMode = LineBreakMode.NoWrap };
          
            stack.Children.Add(label2);
            DetailsStack.Children.Add(stack);
        }

        public void AddStackSq(string attribute2)
        {
            StackLayout stack = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
            Label label2 = new Label { Text = attribute2, FontSize = 18, TextColor = Color.Black, HorizontalOptions = LayoutOptions.FillAndExpand, LineBreakMode = LineBreakMode.NoWrap };
          
            stack.Children.Add(label2);
            DetailsStack.Children.Add(stack);
        }

        public void AddStack(string attribute1, string attribute2)
        {

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            grid.ColumnSpacing = 5;


            //StackLayout stack = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
            Label label1 = new Label { Text = attribute1, FontSize = 13, TextColor = Color.Black, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, LineBreakMode = LineBreakMode.WordWrap };
            //StackLayout layout1 = new StackLayout { WidthRequest = 10 };
            Label label2 = new Label { Text = attribute2, FontSize = 15, TextColor = Color.FromHex("#5CB9E8"), HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, LineBreakMode = LineBreakMode.WordWrap };

           

            grid.Children.Add(label1, 0, 0);
            grid.Children.Add(label2, 1, 0);



            //stack.Children.Add(label1);
            //stack.Children.Add(layout1);
            //stack.Children.Add(label2);
            DetailsStack.Children.Add(grid);
        }


        public void AddApproverStack(string attribute1, string attribute2)
        {
            
            //StackLayout stack = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
            //Label label0 = new Label { Text = "Last Approver", FontSize = 18, TextColor = Color.FromHex("#0C335A"), HorizontalOptions = LayoutOptions.FillAndExpand, LineBreakMode = LineBreakMode.NoWrap };
            ApproverLabel.IsVisible = true;
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            grid.ColumnSpacing = 5;


            //StackLayout stack = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
            Label label1 = new Label { Text = attribute1, FontSize = 13, TextColor = Color.Black, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, LineBreakMode = LineBreakMode.WordWrap };
            //StackLayout layout1 = new StackLayout { WidthRequest = 10 };
            Label label2 = new Label { Text = attribute2, FontSize = 15, TextColor = Color.FromHex("#5CB9E8"), HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, LineBreakMode = LineBreakMode.WordWrap };


           
            grid.Children.Add(label1, 0, 0);
            grid.Children.Add(label2, 1, 0);


            //stack.Children.Add(label0);
            //stack.Children.Add(label1);
            //stack.Children.Add(layout1);
            //stack.Children.Add(label2);
            //ApproverLabel.Children.Add(label0);
            ApproverStack.Children.Add(grid);





          
        }


        public void AddStackBold(string attribute1, string attribute2)
        {

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            grid.ColumnSpacing = 5;

            // StackLayout stack = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
            Label label1 = new Label { Text = attribute1, FontSize = 13, TextColor = Color.Black, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, LineBreakMode = LineBreakMode.WordWrap };
            // StackLayout layout1 = new StackLayout {WidthRequest=10 };
            Label label2 = new Label { Text = attribute2, FontSize = 15, TextColor = Color.FromHex("#5CB9E8"), HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, LineBreakMode = LineBreakMode.WordWrap };
          


            grid.Children.Add(label1, 0, 0);
            grid.Children.Add(label2, 1, 0);

            // stack.Children.Add(label1);
            // stack.Children.Add(layout1);
            // stack.Children.Add(label2);
            DetailsStack.Children.Add(grid);
        }

        public void AddStackAttachments()
        {
            StackLayout stack = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
            Label label2 = new Label { Text = "Attachments", FontSize = 18, TextColor = Color.Black, HorizontalOptions = LayoutOptions.FillAndExpand, LineBreakMode = LineBreakMode.NoWrap };
           
            stack.Children.Add(label2);
            DetailsStack.Children.Add(stack);
        }

        //public void AddStackAttachments(GetAttachment attribute)
        //{
        //    StackLayout stack = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
        //    Button label2 = new Button { Text = attribute.file_name, FontSize = 16, BackgroundColor = Color.FromHex("#3599C9"), TextColor = Color.White, HorizontalOptions = LayoutOptions.FillAndExpand, StyleId = viewModel.GetAttachmentInformation.get_attachment.IndexOf(attribute).ToString() };

        //    string k = viewModel.GetAttachmentInformation.get_attachment.IndexOf(attribute).ToString();

        //    label2.Clicked += async (sender, e) =>
        //    {
        //        int position = Int32.Parse(((Button)sender).StyleId);
        //        Title = "";
        //        await Navigation.PushAsync(new ViewAttachmentScreen(viewModel.GetAttachmentInformation.get_attachment[position].transaction_id, viewModel.GetAttachmentInformation.get_attachment[position].file_id.ToString(), viewModel.GetAttachmentInformation.get_attachment[position].file_content_type, viewModel.GetAttachmentInformation.get_attachment[position].file_name));
        //    };

          
        //    stack.Children.Add(label2);
        //    DetailsStack.Children.Add(stack);
        //}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Title = "Details";
            App.NavigationStackCount = Navigation.NavigationStack.Count;
        }

        //New Action history

        public void AddStackActionLineAH()
        {
            StackLayout stack = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("#D3D3D3"), HeightRequest = 0.5, Margin = new Thickness(10, 0, 10, 0), VerticalOptions = LayoutOptions.Start };
            ActionHistoryStack.Children.Add(stack);
        }

        public void AddStackActionAH()
        {
            StackLayout stack = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
            Label label2 = new Label { Text = "Action History", FontSize = 18, TextColor = Color.Black, HorizontalOptions = LayoutOptions.FillAndExpand, LineBreakMode = LineBreakMode.NoWrap };
          
            stack.Children.Add(label2);
            ActionHistoryStack.Children.Add(stack);
        }


        public void AddStackBoldAH(string attribute1, string attribute2)
        {

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            grid.ColumnSpacing = 5;

            Label label1 = new Label { Text = attribute1, FontSize = 13, TextColor = Color.Black, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, LineBreakMode = LineBreakMode.WordWrap };
            Label label2 = new Label { Text = attribute2, FontSize = 15, TextColor = Color.FromHex("#5CB9E8"), HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, LineBreakMode = LineBreakMode.WordWrap };

           


            grid.Children.Add(label1, 0, 0);
            grid.Children.Add(label2, 1, 0);
            ActionHistoryStack.Children.Add(grid);
        }


        public void AddStackAH(string attribute1, string attribute2)
        {

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            grid.ColumnSpacing = 5;

            Label label1 = new Label { Text = attribute1, FontSize = 13, TextColor = Color.Black, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, LineBreakMode = LineBreakMode.WordWrap };
            Label label2 = new Label { Text = attribute2, FontSize = 15, TextColor = Color.FromHex("#5CB9E8"), HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, LineBreakMode = LineBreakMode.WordWrap };

          
            grid.Children.Add(label1, 0, 0);
            grid.Children.Add(label2, 1, 0);
            ActionHistoryStack.Children.Add(grid);
        }

        public void AddStackActionSeparatorLineAH()
        {
            StackLayout stack = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("#D3D3D3"), HeightRequest = 0.5, Margin = new Thickness(70, 0, 70, 0), VerticalOptions = LayoutOptions.Start };
            ActionHistoryStack.Children.Add(stack);
        }

        public void ShowStackLineButtons()
        {
           
           


            Button InvoiceAmountView = new Button();
            InvoiceAmountView.BackgroundColor = Color.White;
            InvoiceAmountView.SetBinding(Button.IsVisibleProperty, "InvoiceAmountView");
            InvoiceAmountView.BorderColor = Color.Red;
            InvoiceAmountView.SetBinding(Button.CommandProperty, "SubmitInvoiceAmountCommand");
            InvoiceAmountView.BorderRadius = 8;
            InvoiceAmountView.HorizontalOptions = LayoutOptions.Start;
            InvoiceAmountView.VerticalOptions = LayoutOptions.Center;
            InvoiceAmountView.BorderWidth = 2;
            InvoiceAmountView.WidthRequest = 140;
            InvoiceAmountView.HeightRequest = 40;
            InvoiceAmountView.Text = "Amount Summary";

            ButtonStack.Children.Add(InvoiceAmountView);


            Button InvoiceLineView = new Button();
            InvoiceLineView.BackgroundColor = Color.White;
            InvoiceLineView.SetBinding(Button.IsVisibleProperty, "InvoiceLineView");
            InvoiceLineView.BorderColor = Color.Red;
            InvoiceLineView.SetBinding(Button.CommandProperty, "SubmitInvoiceLineCommand");
            InvoiceLineView.BorderRadius = 8;
            InvoiceLineView.HorizontalOptions = LayoutOptions.Start;
            InvoiceLineView.VerticalOptions = LayoutOptions.Center;
            InvoiceLineView.BorderWidth = 2;
            InvoiceLineView.WidthRequest = 140;
            InvoiceLineView.HeightRequest = 40;
            InvoiceLineView.Text = "Invoice Lines";

            ButtonStack.Children.Add(InvoiceLineView);

            Button InvoiceDistributionView = new Button();
            InvoiceDistributionView.BackgroundColor = Color.White;
            InvoiceDistributionView.SetBinding(Button.IsVisibleProperty, "InvoiceDistributionView");
            InvoiceDistributionView.BorderColor = Color.Red;
            InvoiceDistributionView.SetBinding(Button.CommandProperty, "SubmitInvoiceDistributionCommand");
            InvoiceDistributionView.BorderRadius = 8;
            InvoiceDistributionView.HorizontalOptions = LayoutOptions.Start;
            InvoiceDistributionView.VerticalOptions = LayoutOptions.Center;
            InvoiceDistributionView.BorderWidth = 2;
            InvoiceDistributionView.WidthRequest = 140;
            InvoiceDistributionView.HeightRequest = 40;
            InvoiceDistributionView.Text = "Distribution Lines";

            ButtonStack.Children.Add(InvoiceDistributionView);


            Button RequisitionLineView = new Button();
            RequisitionLineView.BackgroundColor = Color.White;
            RequisitionLineView.SetBinding(Button.IsVisibleProperty, "RequisitionLineView");
            RequisitionLineView.BorderColor = Color.Red;
            RequisitionLineView.SetBinding(Button.CommandProperty, "SubmitRequisitionLineCommand");
            RequisitionLineView.BorderRadius = 8;
            RequisitionLineView.HorizontalOptions = LayoutOptions.Start;
            RequisitionLineView.VerticalOptions = LayoutOptions.Center;
            RequisitionLineView.BorderWidth = 2;
            RequisitionLineView.WidthRequest = 140;
            RequisitionLineView.HeightRequest = 40;
            RequisitionLineView.Text = "Line Details";

            ButtonStack.Children.Add(RequisitionLineView);



            Button RequisitionDistributionView = new Button();
            RequisitionDistributionView.BackgroundColor = Color.White;
            RequisitionDistributionView.SetBinding(Button.IsVisibleProperty, "RequisitionDistributionView");
            RequisitionDistributionView.BorderColor = Color.Red;
            RequisitionDistributionView.SetBinding(Button.CommandProperty, "SubmitRequisitionDistributionCommand");
            RequisitionDistributionView.BorderRadius = 8;
            RequisitionDistributionView.HorizontalOptions = LayoutOptions.Start;
            RequisitionDistributionView.VerticalOptions = LayoutOptions.Center;
            RequisitionDistributionView.BorderWidth = 2;
            RequisitionDistributionView.WidthRequest = 140;
            RequisitionDistributionView.HeightRequest = 40;
            RequisitionDistributionView.Text = "Distribution Lines";

            ButtonStack.Children.Add(RequisitionDistributionView);


            Button JournalBadgeView = new Button();
            JournalBadgeView.BackgroundColor = Color.White;
            JournalBadgeView.SetBinding(Button.IsVisibleProperty, "JournalBadgeView");
            JournalBadgeView.BorderColor = Color.Red;
            JournalBadgeView.SetBinding(Button.CommandProperty, "SubmitJournalBadgeCommand");
            JournalBadgeView.BorderRadius = 8;
            JournalBadgeView.HorizontalOptions = LayoutOptions.Start;
            JournalBadgeView.VerticalOptions = LayoutOptions.Center;
            JournalBadgeView.BorderWidth = 2;
            JournalBadgeView.WidthRequest = 140;
            JournalBadgeView.HeightRequest = 40;
            JournalBadgeView.Text = "Line Details";

            ButtonStack.Children.Add(JournalBadgeView);



           


              

            Button POLineView = new Button();
            POLineView.BackgroundColor = Color.White;
            POLineView.SetBinding(Button.IsVisibleProperty, "POLineView");
            POLineView.BorderColor = Color.Red;
            POLineView.SetBinding(Button.CommandProperty, "SubmitPOLineCommand");
            POLineView.BorderRadius = 8;
            POLineView.HorizontalOptions = LayoutOptions.Start;
            POLineView.VerticalOptions = LayoutOptions.End;
            POLineView.BorderWidth = 2;
            POLineView.WidthRequest = 140;
            POLineView.HeightRequest = 40;
            POLineView.Text = "Line Details";

            ButtonStack.Children.Add(POLineView);

       


    }












    }
}