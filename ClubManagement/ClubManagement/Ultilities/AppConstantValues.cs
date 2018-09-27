using Android;
using ClubManagement.Models;
using System.Collections.Generic;

namespace ClubManagement.Ultilities
{
    public static class AppConstantValues
    {
        public const string UserIdPreferenceKey = "UserId";

        public const string LogStatusPreferenceKey = "IsLogged";

        public const string UserAvatarUrl = "UserAvatarUrl";

        public const string BalanceFragmentSummaryTab = "Summary";

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

        public static List<FeeOrOutcomeGroupModel> FeeGrooups
        {
            get => new List<FeeOrOutcomeGroupModel>()
            {
                new FeeOrOutcomeGroupModel()
                {
                    Id = "1",
                    ImageId =Resource.Drawable.money_group,
                    TitleId = Resource.String.party
                },
                new FeeOrOutcomeGroupModel()
                {
                    Id = "2",
                    ImageId =Resource.Drawable.money_group,
                    TitleId = Resource.String.sport
                },
                new FeeOrOutcomeGroupModel()
                {
                    Id = "3",
                    ImageId =Resource.Drawable.money_group,
                    TitleId = Resource.String.travelling
                },
                new FeeOrOutcomeGroupModel()
                {
                    Id = "4",
                    ImageId =Resource.Drawable.money_group,
                    TitleId = Resource.String.food_and_drinks
                },
                new FeeOrOutcomeGroupModel()
                {
                    Id = "5",
                    ImageId =Resource.Drawable.money_group,
                    TitleId = Resource.String.gift
                },
                new FeeOrOutcomeGroupModel()
                {
                    Id = "6",
                    ImageId =Resource.Drawable.money_group,
                    TitleId = Resource.String.others
                }
            };
        }
    }
}