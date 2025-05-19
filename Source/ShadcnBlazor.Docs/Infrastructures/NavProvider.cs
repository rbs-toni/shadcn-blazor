using Microsoft.AspNetCore.Components.Routing;

namespace ShadcnBlazor.Docs;

public class NavProvider
{
    public NavProvider()
    {
        // Initialize the Sidebar collection and ensure items are ordered
        SidebarGroups = new List<SidebarGroup>()
        {
            new SidebarGroup
            {
                Title = "Getting Started",
                Order = 1,
                Items =
                    new List<SidebarItem>
                    {
                        new()
                        {
                            Title = "Introduction",
                            Order = 1,
                            Href = "/docs",
                            Match = NavLinkMatch.All,
                            ProgressState = ProgressState.InProgress
                        },
                        new()
                        {
                            Title = "Installation",
                            Order = 2,
                            Href = "/docs/installation",
                            ProgressState = ProgressState.InProgress
                        },
                        new() { Title = "Theming", Order = 3, Href = "/docs/theming" },
                        new() { Title = "Dark mode", Order = 4, Href = "/docs/dark-mode" },
                        new()
                        {
                            Title = "Typography",
                            Order = 5,
                            Href = "/docs/typography",
                            ProgressState = ProgressState.InProgress
                        },
                        new() { Title = "Changelog", Order = 6, Href = "/docs/changelog" },
                        new() { Title = "About", Order = 7, Href = "/docs/about" },
                        new() { Title = "Contribution", Order = 8, Href = "/docs/contribution" },
                        new() { Title = "Usage", Order = 9, Href = "/docs/usage" }
                    }.OrderBy(item => item.Order)  // Ensures the items are ordered correctly by the Order property
                        .ToList()  // Converts back to list after ordering
            },
            new SidebarGroup
            {
                Title = "Components",
                Order = 2,
                Items =
                    new List<SidebarItem>
                    {
                        new()
                        {
                            Title = "Accordion",
                            Order = 1,
                            Href = "/docs/components/accordion",
                            Matches = ["/docs/components", "/docs/components/accordion"],
                            ProgressState = ProgressState.InProgress
                        },
                        new()
                        {
                            Title = "Alert",
                            Order = 2,
                            Href = "/docs/components/alert",
                            ProgressState = ProgressState.InProgress
                        },
                        new() { Title = "Alert Dialog", Order = 3, Href = "/docs/components/alert-dialog" },
                        new()
                        {
                            Title = "Aspect Ratio",
                            Order = 4,
                            Href = "/docs/components/aspect-ratio",
                            ProgressState = ProgressState.InProgress
                        },
                        new()
                        {
                            Title = "Avatar",
                            Order = 5,
                            Href = "/docs/components/avatar",
                            ProgressState = ProgressState.InProgress
                        },
                        new()
                        {
                            Title = "Badge",
                            Order = 6,
                            Href = "/docs/components/badge",
                            ProgressState = ProgressState.InProgress
                        },
                        new()
                        {
                            Title = "Breadcrumb",
                            Order = 7,
                            Href = "/docs/components/breadcrumb",
                            ProgressState = ProgressState.InProgress
                        },
                        new()
                        {
                            Title = "Button",
                            Order = 8,
                            Href = "/docs/components/button",
                            ProgressState = ProgressState.InProgress
                        },
                        new()
                        {
                            Title = "Calendar",
                            Order = 9,
                            Href = "/docs/components/calendar",
                            ProgressState = ProgressState.InProgress
                        },
                        new()
                        {
                            Title = "Card",
                            Order = 10,
                            Href = "/docs/components/card",
                            ProgressState = ProgressState.InProgress
                        },
                        new()
                        {
                            Title = "Carousel",
                            Order = 11,
                            Href = "/docs/components/carousel",
                            ProgressState = ProgressState.InProgress
                        },
                        new() { Title = "Chart", Order = 12, Href = "/docs/components/chart" },
                        new()
                        {
                            Title = "Checkbox",
                            Order = 13,
                            Href = "/docs/components/checkbox",
                            ProgressState = ProgressState.InProgress
                        },
                        new()
                        {
                            Title = "Collapsible",
                            Order = 14,
                            Href = "/docs/components/collapsible",
                            ProgressState = ProgressState.InProgress
                        },
                        new() { Title = "Combobox", Order = 15, Href = "/docs/components/combobox" },
                        new() { Title = "Command", Order = 16, Href = "/docs/components/command" },
                        new() { Title = "Context Menu", Order = 17, Href = "/docs/components/context-menu" },
                        new() { Title = "Data Table", Order = 18, Href = "/docs/components/data-table" },
                        new() { Title = "Date Picker", Order = 19, Href = "/docs/components/date-picker" },
                        new()
                        {
                            Title = "Dialog",
                            Order = 20,
                            Href = "/docs/components/dialog",
                            ProgressState = ProgressState.InProgress
                        },
                        new() { Title = "Drawer", Order = 21, Href = "/docs/components/drawer" },
                        new() { Title = "Dropdown Menu", Order = 22, Href = "/docs/components/dropdown-menu" },
                        new() { Title = "Form", Order = 23, Href = "/docs/components/form" },
                        new() { Title = "Hover Card", Order = 24, Href = "/docs/components/hover-card" },
                        new()
                        {
                            Title = "Input",
                            Order = 25,
                            Href = "/docs/components/input",
                            ProgressState = ProgressState.InProgress
                        },
                        new() { Title = "Input OTP", Order = 26, Href = "/docs/components/input-otp" },
                        new() { Title = "Label", Order = 27, Href = "/docs/components/label" },
                        new() { Title = "Menubar", Order = 28, Href = "/docs/components/menubar" },
                        new() { Title = "Navigation Menu", Order = 29, Href = "/docs/components/navigation-menu" },
                        new() { Title = "Pagination", Order = 30, Href = "/docs/components/pagination" },
                        new()
                        {
                            Title = "Popover",
                            Order = 31,
                            Href = "/docs/components/popover",
                            ProgressState = ProgressState.InProgress
                        },
                        new() { Title = "Progress", Order = 32, Href = "/docs/components/progress" },
                        new() { Title = "Radio Group", Order = 33, Href = "/docs/components/radio-group" },
                        new()
                        {
                            Title = "Resizable",
                            Order = 34,
                            Href = "/docs/components/resizable",
                            //ProgressState = ProgressState.InProgress
                        },
                        new()
                        {
                            Title = "Scroll Area",
                            Order = 35,
                            Href = "/docs/components/scroll-area",
                            ProgressState = ProgressState.InProgress
                        },
                        new() { Title = "Select", Order = 36, Href = "/docs/components/select" },
                        new()
                        {
                            Title = "Separator",
                            Order = 37,
                            Href = "/docs/components/separator",
                            ProgressState = ProgressState.InProgress
                        },
                        new() { Title = "Sheet", Order = 38, Href = "/docs/components/sheet" },
                        new() { Title = "Sidebar", Order = 39, Href = "/docs/components/sidebar" },
                        new()
                        {
                            Title = "Skeleton",
                            Order = 40,
                            Href = "/docs/components/skeleton",
                            ProgressState = ProgressState.InProgress
                        },
                        new() { Title = "Slider", Order = 41, Href = "/docs/components/slider" },
                        new()
                        {
                            Title = "Switch",
                            Order = 43,
                            Href = "/docs/components/switch",
                            ProgressState = ProgressState.InProgress
                        },
                        new()
                        {
                            Title = "Table",
                            Order = 44,
                            Href = "/docs/components/table",
                            ProgressState = ProgressState.InProgress
                        },
                        new()
                        {
                            Title = "Tabs",
                            Order = 45,
                            Href = "/docs/components/tabs",
                            ProgressState = ProgressState.InProgress
                        },
                        new()
                        {
                            Title = "Textarea",
                            Order = 46,
                            Href = "/docs/components/textarea",
                            ProgressState = ProgressState.InProgress
                        },
                        new()
                        {
                            Title = "Toast",
                            Order = 47,
                            Href = "/docs/components/toast",
                            ProgressState = ProgressState.InProgress
                        },
                        new()
                        {
                            Title = "Toggle",
                            Order = 48,
                            Href = "/docs/components/toggle",
                            ProgressState = ProgressState.InProgress
                        },
                        new() { Title = "Toggle Group", Order = 49, Href = "/docs/components/toggle-group" },
                        new()
                        {
                            Title = "Tooltip",
                            Order = 50,
                            Href = "/docs/components/tooltip",
                            ProgressState = ProgressState.InProgress
                        }
                    }.OrderBy(item => item.Order)  // Sort by Order property for sidebar items
                        .ToList()  // Convert back to List after sorting
            }
        };

        // Initialize the NavItems collection
        NavItems = new List<NavItem>
        {
            new() { Title = "Docs", Href = "/docs/installation" },
            new() { Title = "Components", Href = "/docs/components" },
            new() { Title = "Blocks", Href = "/blocks" },
            new() { Title = "Charts", Href = "/charts" },
            new() { Title = "Themes", Href = "/themes" },
            new() { Title = "Colors", Href = "/colors" }
        };
    }

    public List<NavItem> NavItems { get; init; }

    public List<SidebarGroup> SidebarGroups { get; init; }
}
