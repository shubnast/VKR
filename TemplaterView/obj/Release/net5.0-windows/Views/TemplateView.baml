   M S B A M L   `   `   `  ���� H  DTemplaterView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:TemplaterView.ViewsTemplaterView.Views  &     TemplaterView.Views.TemplateView   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     3local!clr-namespace:TemplaterView.Views     _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  ^ ZCaliburn.Micro.Platform, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f"calhttp://caliburnmicro.com    � Title$  Шаблоны��$	��600��$	��700�� � WindowStartupLocation$ CenterScreen=�*  $Microsoft.Xaml.Behaviors.Interaction   Triggers +  %Microsoft.Xaml.Behaviors.EventTrigger     	EventName$ Loaded��*  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. "  Caliburn.Micro.ActionMessage     
MethodName$ 
Initialize��.��� .+��  � ColumnDefinitions �� $
��0.7* ��� .+�� $��0�� � RowDefinitions �� $
��0.3* ��� �� $
��0.3* �.+�� $��1�� �� $
��0.7* ���  �� $
��0.3* ��� $
��0.3* ��� $
��0.3* ��� $
��0.3* ��� .+��� $��5q�$��0��.��Шаблон�� -    ��Name$
 Path��$��1��$��0��$��5q�$	��120��$
��Left=��� -   	 ��Name$	 TemplatePath��$��0��$��1��$��5q�$��25��$��Right=�.��...� $��2�� �� �� �� .+��� -   $	 CreateNewData��$��Bottom=�$��5q�$��30��$��0��{��� *IsCreateEnabled+.��Создать�� -   $	 SaveData��$��Bottom=�$��5q�$��30��$��1��{��� *IsEditEnabled+.��Сохранить�� -   $	 
DeleteData��$��Bottom=�$��5q�$��30��$��2��{��� *IsDeleteEnabled+.��Удалить�� $��0��$��Center=�$��10q�.��Шаблоны�� -   
 ��Name$
 ListViewCollection��$��1�� �� SelectedItem �� *SelectedItem+   $ SelectionChanged��.   $ ItemSelected�� �� View �� .P���  �� Width$	 200�� �� DisplayMemberBinding �� *Path+.��Шаблон