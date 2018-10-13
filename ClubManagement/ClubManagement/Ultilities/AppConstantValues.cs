using ClubManagement.Models;
using System.Collections.Generic;

namespace ClubManagement.Ultilities
{
    public static class AppConstantValues
    {
        public const string UserIdPreferenceKey = "UserId";

        public const string LogStatusPreferenceKey = "IsLogged";

        public const string UserAvatarUrl = "UserAvatarUrl";

        public const string BalanceFragmentSummaryTab = "Total";

        public const string BalanceFragmentOutcomeTab = "Outcome";

        public const string BalanceFragmentIncomeTab = "Income";

        public const float DefaultMapZoomLevel = 12f;

        public const string EventClickShowGoingEventsTabTag = "Going events";

        public const string EventClickShowNewEventsTabTag = "New events";

        public const string EventClickShowMoneyScreenTag = "Money screen";

        public const string EventListHeaderToday = "Today";

        public const string EventListHeaderTomorrow = "Tomorrow";

        public const string EventListHeaderNextWeek = "Next week";

        public const string EventListHeaderOther = "Other";

        public const string EventListHeaderThisWeek = "This week";

        public const string NotificationEditEvent = "Edit event";

        public const string NotificationEditFee = "Edit fee";

        public const string NotificationDeleteEvent = "Delete event";

        public const string NotificationDeleteFee = "Delete fee";

        public const string NotificationPaid = "Paid";

        public const string NotificationRepaid = "Repaid";

        public static List<FeeOrOutcomeGroupModel> FeeGrooups
        {
            get => new List<FeeOrOutcomeGroupModel>()
            {
                new FeeOrOutcomeGroupModel()
                {
                    Id = "1",
                    ImageId =Resource.Drawable.choose_group_party,
                    TitleId = Resource.String.party
                },
                new FeeOrOutcomeGroupModel()
                {
                    Id = "2",
                    ImageId =Resource.Drawable.choose_group_sport,
                    TitleId = Resource.String.sport
                },
                new FeeOrOutcomeGroupModel()
                {
                    Id = "3",
                    ImageId =Resource.Drawable.choose_group_travel,
                    TitleId = Resource.String.travelling
                },
                new FeeOrOutcomeGroupModel()
                {
                    Id = "4",
                    ImageId =Resource.Drawable.choose_group_food_and_drink,
                    TitleId = Resource.String.food_and_drinks
                },
                new FeeOrOutcomeGroupModel()
                {
                    Id = "5",
                    ImageId =Resource.Drawable.choose_group_gift,
                    TitleId = Resource.String.gift
                },
                new FeeOrOutcomeGroupModel()
                {
                    Id = "6",
                    ImageId =Resource.Drawable.choose_group_others,
                    TitleId = Resource.String.others
                }
            };
        }

        public static Dictionary<string,string> NotificationStartBold
        {
            get => new Dictionary<string, string>()
            {
                { NotificationEditEvent, "event" },
                { NotificationEditFee, "fee" },
                { NotificationDeleteEvent, "event" },
                { NotificationDeleteFee, "fee" },
                { NotificationPaid, "fee" },
                { NotificationRepaid, "fee" }
            };
        }

        public static Dictionary<string, string> NotificationEndBold
        {
            get => new Dictionary<string, string>()
            {
                { NotificationDeleteEvent, "was" },
                { NotificationDeleteFee, "was" },
                { NotificationPaid, "for" },
                { NotificationRepaid, "for" }
            };
        }
    }
}