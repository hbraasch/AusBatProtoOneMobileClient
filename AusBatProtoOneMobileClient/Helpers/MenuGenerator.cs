using Mobile.ViewModels;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using TreeApp.Helpers;
using Xamarin.Forms;
using static Mobile.Helpers.MenuGenerator;

namespace Mobile.Helpers
{
    public class MenuGenerator: BindableObject
    {

        public MenuUnits menuUnits = new MenuUnits();

        public static BindableProperty InvalidateCommandProperty =
                BindableProperty.Create(nameof(InvalidateCommand), typeof(ICommand), typeof(MenuGenerator), null, BindingMode.OneWayToSource);

        public ICommand InvalidateCommand
        {
            get { return (ICommand)GetValue(InvalidateCommandProperty); }
            set { SetValue(InvalidateCommandProperty, value); }
        }


        public MenuGenerator Configure()
        {
            InvalidateCommand = new Command(() =>
            {
                this.Invalidate();
            });
            return this;
        }


        // Add to top menu
        internal MenuGenerator AddMenuItem(string id, string name, ToolbarItemOrder order, Action<MenuUnit> onClicked = null, bool isVisible = true, string iconPath = "")
        {
            var newMenuUnit = new MenuUnit(id, this);
            newMenuUnit.name = name;
            newMenuUnit.order = order;
            newMenuUnit.onClicked = onClicked;
            newMenuUnit.isVisible = isVisible;
            newMenuUnit.iconPath = iconPath;
            menuUnits.Add(newMenuUnit);
            return this;
        }

        // Add submenu
        internal MenuGenerator AddSubMenuItem(string parentId, string id, string name, ToolbarItemOrder order, Action<MenuUnit> onClicked = null)
        {
            var parentMenuUnit = FindMenuUnit(parentId);
            var newMenuUnit = new MenuUnit(id, this);
            newMenuUnit.name = name;
            newMenuUnit.order = order;
            newMenuUnit.onClicked = onClicked;
            parentMenuUnit.menuUnits.Add(newMenuUnit);
            return this;
        }

        



        Page page;
        private const string ELLIPSES = "...";

        private string SPECIAL_CANCEL_STRING = $"Cancel";
        public void GenerateToolbarItemsForPage(Page page)
        {

            lock (page.ToolbarItems)
            {
                if (page == null) return;

                this.page = page;

                bool IsHamburgerItem = false;


                #region *// Process visibility

                if (visibilityManager != null)
                {
                    visibilityManager.Init();
                    visibilityManager.EvaluateFactors();
                    GetMenuUnits().ForEach(o=>o.thisUnit.isVisible = visibilityManager.IsMenuUnitVisible(o.thisUnit, true));
                }

                #endregion


                page.ToolbarItems.Clear();
                foreach (var rootMenuUnit in menuUnits)
                {
                    string ellipses = (rootMenuUnit.menuUnits.IsEmpty()) ? "" : ELLIPSES;   // Add ellipses to menu text if item has submenus

                    if (rootMenuUnit.isVisible)
                    {
                        if (Device.RuntimePlatform == Device.iOS)
                        {
                            // iOS: Needs to manually add Hamburger functionality
                            if (rootMenuUnit.order == ToolbarItemOrder.Primary)
                            {
                                // Display in top bar
                                ToolbarItem toolbarItem = new ToolbarItem { Text = $"{rootMenuUnit.name} {ellipses}", IconImageSource = rootMenuUnit.iconPath, Order = ToolbarItemOrder.Primary };
                                toolbarItem.Command = new Command(() => { HandleMenuEntityClick(rootMenuUnit.id); });
                                toolbarItem.CommandParameter = rootMenuUnit.id;
                                page.ToolbarItems.Add(toolbarItem);
                            }
                            else
                            {
                                // Display inside hamburger
                                IsHamburgerItem = true;
                            }
                        }
                        else
                        {

                            // Android: Hamburger functionality is built-in
                            ToolbarItem toolbarItem = new ToolbarItem { Text = $"{rootMenuUnit.name} {ellipses}", IconImageSource = rootMenuUnit.iconPath };
                            toolbarItem.Order = rootMenuUnit.order;
                            toolbarItem.Command = new Command(() => { HandleMenuEntityClick(rootMenuUnit.id); });
                            toolbarItem.CommandParameter = rootMenuUnit.id;
                            if (Device.RuntimePlatform == Device.UWP)
                            {
                                if (!rootMenuUnit.iconPath.IsEmpty())
                                {
                                    toolbarItem.IconImageSource = rootMenuUnit.iconPath;
                                }
                            }
                            page.ToolbarItems.Add(toolbarItem);

                        }
                    } 
                } 

                #region *// Add the hamburger icon in case its simulated
                if (Device.RuntimePlatform == Device.iOS)
                {
                    if (IsHamburgerItem)
                    {
                        ToolbarItem toolbarItem = new ToolbarItem(ELLIPSES, "ic_hamburger.png", () => { HandleSecondaryItemClick(); }, ToolbarItemOrder.Primary);
                        page.ToolbarItems.Add(toolbarItem);
                    }
                }

                #endregion


            }
        }

        internal MenuGenerator AddMenuItem(string v1, string v2, string v3, ToolbarItemOrder secondary, Action<object> p)
        {
            throw new NotImplementedException();
        }

        private List<(MenuUnit parentUnit, MenuUnit thisUnit)> GetMenuUnits()
        {
            List<(MenuUnit parentUnit, MenuUnit unit)> menuUnits = new List<(MenuUnit parentUnit, MenuUnit unit)>();
            var traverser = new MenuTraverser(this.menuUnits);
            traverser.Execute((parentUnit, unit) =>
            {
                menuUnits.Add((parentUnit, unit));
                return false;
            });
            return menuUnits;
        }

        /// <summary>
        /// Not required anymore
        /// </summary>
        /// <param name="menuUnitId"></param>
        /// <param name="path"></param>
        internal void AddUwpIcon(string menuUnitId, string path)
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                MenuUnit foundMenuUnit = FindMenuUnit(menuUnitId);
                foundMenuUnit.iconPath = path;
            }
        }

        private async void HandleMenuEntityClick(String menuEntityId)
        {
            MenuUnit foundMenuEntity = FindMenuUnit(menuEntityId);
            if (foundMenuEntity.menuUnits.IsEmpty())
            {
                // Leaf node, so fire event
                if (foundMenuEntity.onClicked != null)
                {
                    foundMenuEntity.onClicked?.Invoke(foundMenuEntity);
                }
            }
            else
            {
                // Branch node, so display sub menu
                // Rename where required
                foreach (var menuItem in foundMenuEntity.menuUnits)
                {
                    if (!menuItem.menuUnits.IsEmpty())
                    {
                        if (!menuItem.name.Contains(ELLIPSES))
                        {
                            menuItem.name += " " + ELLIPSES;
                        }
                    }
                }
                String[] submenuNames = foundMenuEntity.menuUnits.Where(x => x.isVisible).Select(x => x.name).ToArray();
                String[] submenuIds = foundMenuEntity.menuUnits.Where(x => x.isVisible).Select(x => x.id).ToArray();
                var sheetTitle = foundMenuEntity.name;
                if (!sheetTitle.Contains(ELLIPSES))
                {
                    sheetTitle += " " + ELLIPSES;
                }
                var action = await page.DisplayActionSheet(sheetTitle, SPECIAL_CANCEL_STRING, null, submenuNames);
                if (action != SPECIAL_CANCEL_STRING)
                {
                    for (int i = 0; i < submenuNames.Length; i++)
                    {
                        if (submenuNames[i] == action)
                        {
                            HandleMenuEntityClick(submenuIds[i]);
                            break;
                        }
                    }
                }
            }
        }



        private async void HandleSecondaryItemClick()
        {

            foreach (var menuItem in menuUnits)
            {
                if (!menuItem.menuUnits.IsEmpty())
                {
                    if (!menuItem.name.Contains(ELLIPSES))
                    {
                        menuItem.name += $" {ELLIPSES}";
                    }
                }
            }
            String[] submenuNames = menuUnits.Where(x => x.order == ToolbarItemOrder.Secondary).Where(x => x.isVisible).Select(x => x.name).ToArray();
            String[] submenuIds = menuUnits.Where(x => x.order == ToolbarItemOrder.Secondary).Where(x => x.isVisible).Select(x => x.id).ToArray();
            var action = await page.DisplayActionSheet($"Menu ...", SPECIAL_CANCEL_STRING, null, submenuNames);
            if (action != SPECIAL_CANCEL_STRING)
            {
                for (int i = 0; i < submenuNames.Length; i++)
                {
                    if (submenuNames[i] == action)
                    {
                        HandleMenuEntityClick(submenuIds[i]);
                        break;
                    }
                }
            }
        }

        public MenuUnit FindMenuUnit(string menuId)
        {
            MenuUnit found = null;
            var traverser = new MenuTraverser(menuUnits);
            traverser.Execute((parentItem, item) =>
            {
                if (item.id == menuId)
                {
                    found = item;
                    return true;
                }
                return false;
            });
            if (found == null)
            {
                throw new ApplicationException($"Could not find menu id [{menuId}] when generating menu");
            }
            return found;
        }

        public class MenuTraverser
        {

            private List<MenuUnit> menuEntities;

            public MenuTraverser(List<MenuUnit> MenuEntities)
            {
                menuEntities = MenuEntities;
            }

            /// <summary>
            /// Used to traverse menus. Return true when stopping traverse
            /// </summary>
            /// <param name="onNodeListener"></param>

            public void Execute(Func<MenuUnit, MenuUnit, bool> onNodeListener)
            {
                foreach (var menuEntity in menuEntities)
                {
                    if (GetNextMenuEntityRecursively(null, menuEntity, onNodeListener) == true) return;
                }
            }

            private bool GetNextMenuEntityRecursively(MenuUnit parentMenuEntity, MenuUnit menuEntity, Func<MenuUnit, MenuUnit, bool> onNodeListener)
            {
                if (onNodeListener(parentMenuEntity, menuEntity) == true) return true;
                // TODO: Complete member initialization
                foreach (var submenuEntity in menuEntity.menuUnits)
                {
                    if (GetNextMenuEntityRecursively(menuEntity, submenuEntity, onNodeListener) == true) return true;
                }
                return false;
            }

        }

        public class MenuUnits : List<MenuUnit> { }

        internal void SetVisible(string menuId, bool isVisible)
        {
            var menuItem = FindMenuUnit(menuId);
            if (menuItem == null) throw new ApplicationException($"Menu item '{menuId}' does not exist");
            menuItem.isVisible = isVisible;
        }

        internal void Invalidate()
        {
            Device.BeginInvokeOnMainThread(() => {
                GenerateToolbarItemsForPage(page);
            });

        }


        /// <summary>
        /// Manage visibility of menu units
        ///                         visibilityFactor0, visibilityFactor1, ... visibilityFactorN
        ///         Rule0           Cell                    Cell                    Cell
        ///         Rule1           Cell                    Cell                    Cell
        ///         ...             ...                     ...                     ...
        ///         RuleN           Cell                    Cell                    Cell
        /// </summary>
        public class VisibilityManager
        {
            MenuGenerator menu;
            ViewModelBase viewModel;
            public List<string> visibilityFactors;
            public TruthTable truthTable;   // Contain Cell's which contains Factor method, Factor requirement

            public VisibilityManager(MenuGenerator menu, ViewModelBase viewModel, List<string> visibilityFactors)
            {
                this.menu = menu;
                this.viewModel = viewModel;
                this.visibilityFactors = visibilityFactors.ToList();
            }

            public VisibilityManager ToShowMenuItem(string menuUnitId, params bool?[] ruleRequirements)
            {
                var menuUnit = menu.FindMenuUnit(menuUnitId);
                var newRule = new Rule();
                if (ruleRequirements != null)
                {
                    foreach (var ruleRequirement in ruleRequirements)
                    {
                        newRule.Add(ruleRequirement);
                    } 
                }
                else
                {
                    newRule.Add(null);
                }
                menuUnit.visibilityRules.Add(newRule);
                return this;
            }

            internal bool IsMenuUnitVisible(MenuUnit unit, bool isVerbose)
            {
                return truthTable.IsMenuUnitVisible(unit, isVerbose);
            }

            internal void EvaluateFactors()
            {
                truthTable.Evaluate();
            }

            internal void Init()
            {
                if (truthTable == null)
                {
                    truthTable = new TruthTable(menu, viewModel, visibilityFactors);
                }
            }

            public class Rule : List<bool?> { }

            public class Rules : List<Rule> { }

            public class TruthTable
            {
                ViewModelBase viewModel;

                public class TruthTableRule
                {
                    public string MenuUnitName { get; set; }
                    public List<TruthTableCell> TruthTableCells = new List<TruthTableCell>();
                }
                public class TruthTableCell
                {
                    public string FactorMethodName { get; set; }
                    public bool? FactorRequirement { get; set; }
                    public bool Evaluation { get; set; }
                }

                List<TruthTableRule> truthTableRules = new List<TruthTableRule>();
                ILookup<string, TruthTableRule> menuUnitVsRuleLookup;

                public TruthTable(MenuGenerator menu, ViewModelBase viewModel, List<string> visibilityFactors)
                {
                    this.viewModel = viewModel;
                    Generate(menu, visibilityFactors);
                }

                private void Generate(MenuGenerator menu, List<string> visibilityFactors)
                {
                    truthTableRules.Clear();
                    var unitDatas = menu.GetMenuUnits();

                    foreach (var unitData in unitDatas)
                    {
                        var menuUnitName = unitData.thisUnit.id;
                        foreach (var factorRequirement in unitData.thisUnit.visibilityRules)
                        {
                            var truthTableCellsForRule = new List<TruthTableCell>();
                            for (int i = 0; i < visibilityFactors?.Count; i++)
                            {
                                var factorMethodName = visibilityFactors[i];
                                var truthTableCell = new TruthTableCell() { FactorMethodName = factorMethodName, FactorRequirement = factorRequirement[i] };
                                truthTableCellsForRule.Add(truthTableCell);
                            }
                            if (!truthTableCellsForRule.IsEmpty())
                            {
                                truthTableRules.Add(new TruthTableRule { MenuUnitName = menuUnitName, TruthTableCells = truthTableCellsForRule });
                            }
                        }
                    }
                }

                public void Evaluate()
                {
                    Dictionary<string, bool> functionResults = new Dictionary<string, bool>();

                    foreach (var truthTableRule in truthTableRules)
                    {
                        foreach (var truthTableCell in truthTableRule.TruthTableCells)
                        {
                            try
                            {
                                bool status;
                                bool evaluation = false;
                                switch (truthTableCell.FactorRequirement)
                                {
                                    case true:
                                        status = GetVisibilityRuleResult(viewModel, ref functionResults, truthTableCell.FactorMethodName);
                                        evaluation = status;
                                        break;
                                    case false:
                                        status = GetVisibilityRuleResult(viewModel, ref functionResults, truthTableCell.FactorMethodName);
                                        evaluation = !status;
                                        break;
                                    case null:
                                        evaluation = true;
                                        break;
                                }
                                truthTableCell.Evaluation = evaluation;
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"{nameof(Evaluate)}, Menu condition evaluator [{truthTableCell.FactorMethodName}] failed", ex);
                                truthTableCell.Evaluation = false;
                            }
                        }
                    }
                    menuUnitVsRuleLookup = truthTableRules.ToLookup(o => o.MenuUnitName);
                }

                /// <summary>
                /// Used to optimize lookup. Only runs a function once
                /// </summary>
                /// <param name="statusHelper"></param>
                /// <param name="functionResults"></param>
                /// <param name="factorDelegateName"></param>
                /// <returns></returns>
                private bool GetVisibilityRuleResult(ViewModelBase viewModel, ref Dictionary<string, bool> functionResults, string factorDelegateName)
                {
                    if (functionResults.ContainsKey(factorDelegateName))
                    {
                        return functionResults[factorDelegateName];
                    }
                    else
                    {                       
                        if (viewModel.GetType().GetProperty(factorDelegateName) != null)
                        {
                            var status = (bool) viewModel.GetType().GetProperty(factorDelegateName).GetValue(viewModel, null);
                            functionResults.Add(factorDelegateName, status);
                            return status;
                        }
                        MethodInfo statusHelperFunction = viewModel.GetType().GetMethod(factorDelegateName, BindingFlags.Instance | BindingFlags.Public);
                        if (statusHelperFunction != null) {
                            var status = (bool)statusHelperFunction.Invoke(viewModel, null);
                            functionResults.Add(factorDelegateName, status);
                            return status;
                        }
                        else
                        {
                            Debug.WriteLine($"Problem in {nameof(GetVisibilityRuleResult)}. Visibility factor delegate [{factorDelegateName}'] could not be found in designated viewmodel [{viewModel.ToString()}]");
                            return false;
                        }
                    }
                }

                public bool IsMenuUnitVisible(MenuUnit menuUnit, bool isVerbose = false)
                {
                    if (menuUnit.visibilityRules.IsEmpty()) return menuUnit.isVisible;

                    List<(bool result, string failReason)> OrResults = new List<(bool, string)>();
                    if (!menuUnitVsRuleLookup.Contains(menuUnit.id))
                    {
                        // No rule, so display always
                        return true;
                    }
                    foreach (var menuUnitRule in menuUnitVsRuleLookup[menuUnit.id])
                    {
                        var logicOrResult = true;
                        var logicOrFailReason = "";
                        foreach (var truthTableCell in menuUnitRule.TruthTableCells)
                        {
                            if (truthTableCell.FactorRequirement != null)
                            {
                                if (truthTableCell.Evaluation == false)
                                {
                                    if (isVerbose) logicOrFailReason += $"{(logicOrFailReason == "" ? "" : ",")}{truthTableCell.FactorMethodName}";
                                    logicOrResult = false;
                                    break;
                                }
                            }
                        }
                        OrResults.Add((logicOrResult, logicOrFailReason));
                    }
                    if (OrResults.Count > 0)
                    {
                        var overallResult = OrResults.Count(o => o.result == true) > 0;
                        if (overallResult == false && isVerbose)
                        {
                            OrResults.ForEach(o => { if (o.result == false) Debug.WriteLine($"MenuUnit [{menuUnit.id}] is invisible because [{o.failReason}] evaluated false"); });
                        }                       
                        return overallResult;
                    }
                    return false;
                }
            }
        }

        VisibilityManager visibilityManager;
        public VisibilityManager SetVisibilityFactors(ViewModelBase viewModel, params string[] factors)
        {
            visibilityManager = new VisibilityManager(this, viewModel, factors.ToList());
            return visibilityManager;
        }


    }

    public class MenuUnit:BindableObject
    {
        public string id;
        public string name;
        public MenuUnits menuUnits = new MenuUnits();
        public ToolbarItemOrder order;
        public Action<MenuUnit> onClicked;
        public string iconPath;
        public bool isVisible;
        public MenuGenerator menuGenerator;

        public VisibilityManager.Rules visibilityRules { get; internal set; } = new VisibilityManager.Rules();

        public MenuUnit(string id, MenuGenerator menuGenerator)
        {
            this.id = id;
            this.menuGenerator = menuGenerator;
            isVisible = true;
        }

        public static BindableProperty NameTextProperty =
            BindableProperty.Create(nameof(NameText), typeof(string), typeof(MenuUnit), "", BindingMode.TwoWay, propertyChanged: OnNameTextChanged);

        [SuppressPropertyChangedWarnings]
        private static void OnNameTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var context = (MenuUnit)bindable;
            var value = (string)newValue;
            if (string.IsNullOrEmpty(value)) return;
            context.name = value;
            context.menuGenerator.Invalidate();
        }

        public string  NameText
        {
            get { return (string)GetValue(NameTextProperty); }
            set { SetValue(NameTextProperty, value); }
        }
    }
}
