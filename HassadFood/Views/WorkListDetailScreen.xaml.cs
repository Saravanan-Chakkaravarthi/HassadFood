using System;
using System.Collections.Generic;
using HassadFood.ViewModels;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.Views
{
    public partial class WorkListDetailScreen : ContentPage
    {
        WorkListDetailModel viewModel;
        public WorkListDetailScreen(int position)
        {
            InitializeComponent();
            FlowDirection = Resx.AppResources.Culture.Parent.Name.ToString().Equals("ar") ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            BindingContext = viewModel = new WorkListDetailModel(this, position);
            if (viewModel.SelectedItemWorkList.action != "N")
            {
                ApproverView.IsVisible = true;
                NotificationView1.IsVisible = false;
                NotificationView2.IsVisible = false;
            }
            else
            {
                if (viewModel.SelectedItemWorkList.selected_person_id > 0)
                {
                    ApproverView.IsVisible = false;
                    NotificationView1.IsVisible = true;
                    NotificationView2.IsVisible = true;
                }
                else
                {
                    ApproverView.IsVisible = true;
                    NotificationView1.IsVisible = false;
                    NotificationView2.IsVisible = false;
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Title = "Details";
            App.NavigationStackCount = Navigation.NavigationStack.Count;
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

        //public void AddStackAction()
        //{
        //    StackLayout stack = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
        //    Label label2 = new Label { Text = "Action History", FontSize = 16, TextColor = Color.Black, HorizontalOptions = LayoutOptions.FillAndExpand, LineBreakMode = LineBreakMode.NoWrap };

        //    stack.Children.Add(label2);
        //    DetailsStack.Children.Add(stack);
        //}

        public void AddStack(string attribute1, string attribute2)
        {
            StackLayout stack = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
            Label label1 = new Label { Text = attribute1, FontSize = 14, TextColor = Color.FromHex("#229ed8"), VerticalTextAlignment= TextAlignment.Center, LineBreakMode = LineBreakMode.WordWrap };
            Label label2 = new Label { Text = attribute2, FontSize = 14, TextColor = Color.Black, HorizontalOptions = LayoutOptions.FillAndExpand, LineBreakMode = LineBreakMode.WordWrap };

            stack.Children.Add(label1);
            stack.Children.Add(label2);
            DetailsStack.Children.Add(stack);
        }

        public void AddStackAttachments()
        {
            StackLayout stack = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
            Label label2 = new Label { Text = "Attachments", FontSize = 16, TextColor = Color.Black, HorizontalOptions = LayoutOptions.FillAndExpand, LineBreakMode = LineBreakMode.NoWrap };
            stack.Children.Add(label2);
            DetailsStack.Children.Add(stack);
        }

        public void AddStackAttachments(GetAttachment attribute)
        {
            StackLayout stack = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.Start };
            Button label2 = new Button { Text = attribute.file_name, FontSize = 16, TextColor = Color.FromHex("#5f6f50"), HorizontalOptions = LayoutOptions.FillAndExpand, StyleId = viewModel.GetAttachmentInformation.get_attachment.IndexOf(attribute).ToString() };

            label2.Clicked += async (sender, e) =>
            {
                int position = Int32.Parse(((Button)sender).StyleId);
                Title = "";
                await Navigation.PushAsync(new ViewAttachmentScreen(viewModel.GetAttachmentInformation.get_attachment[position].transaction_id, viewModel.GetAttachmentInformation.get_attachment[position].file_id.ToString(), viewModel.GetAttachmentInformation.get_attachment[position].file_content_type));
            };

            stack.Children.Add(label2);
            DetailsStack.Children.Add(stack);
        }
    }
}
