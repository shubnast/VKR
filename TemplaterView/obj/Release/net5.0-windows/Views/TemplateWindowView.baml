   M S B A M L   `   `   `  ����   TemplaterView,    &TemplaterView.Views.TemplateWindowView   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  ^ ZCaliburn.Micro.Platform, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f"calhttp://caliburnmicro.com    � Title$  Шаблонизатор�� � WindowStartupLocation$ CenterScreen=�$	��450��$	��800��*  $Microsoft.Xaml.Behaviors.Interaction   Triggers +  %Microsoft.Xaml.Behaviors.EventTrigger     	EventName$ Loaded��*  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. "  Caliburn.Micro.ActionMessage     
MethodName$ 
Initialize��.��� .+�� -    �Name$ MainRoot�� � RowDefinitions �� -    ��Name$ First��$��20 ��� -   $ Second��$��30 ��� -   $ Third��	 � ColumnDefinitions	 �� .+�o� $��0��$��0��$��20��$	��Top=�.@�m� $��Файл��.@�m� -   
 m�Name$
 ExitMenuClick��$��
Выход��m� $��Объект��.@�m� -   $
 SubjectMenuClick��$��Персона��m� -   $
 CourseMenuClick��$��Курс��m� -   $
 DepartamentMenuClick��$��Кафедра��m� -	   $ 
 TrainingDirectionMenuClick��$1��+Направление подготовки��m� -
   $
 PreparationTypeMenuClick��$4��.Тип подготовик (обучения)��m� $��Закрепления��.@�m� -   $
 GroupMenuClick��$��Группа��m� -   $
 StudentGroupMenuClick��$(��"Студент за группой��m� -   $
 LecturerGroupMenuClick��$4��.Преподаватель за группой��m� $��Настройки��.@�m� -   $
 TemplateMenuClick��$��Шаблоны��m� -   $
 SettingsMenuClick��$��Константы���� $��1�� �� Orientation$ 
Horizontal=�.+��� -    ��Name$ 	btnCreate��$��1��$��3q�$��30��$
��Left=�����  j� Stretch$ UniformToFill=� �� ImageSource$ /Views/add.png���� -   $ btnEdit��$��1��$��3q�$��30��$
��Left=����� $ /Views/edit.png���� -   $ 	btnDelete��$��1��$��3q�$��30��$
��Left=����� $ /Views/close.png���� -   $ btnPrint��$��1��$��3q�$��30��$
��Left=����� $ /Views/print.png���� .�� Печать по группе�� -    ��Name$ ListBoxCollection��$	��100��$��3q� �� SelectedItem �� *SelectedGroupItem+���� .R��� ���� �� -    ��Name$ ListViewCollection��$��1��$��2�� �� SelectionMode$ Extended=� �� *SelectedItem+   $ SelectionChanged��.   $ ItemSelected�� � 	Resources  �� 
TargetType �� Property �� Value �� Mode��%Z   '��       �� " ��.���  �@��
IsSelected �� *SomeItemSelected+$ TwoWay=� �� View �� .P���  �� Width$	 100�� �� DisplayMemberBinding �� *	Student+.��Студент�� $	 100�� �� *Group+.��Группа�� $	 100�� �� *Course+.��
Курс�� $	 100�� �� *Departament+.��Кафедра�� $	 280�� �� *TrainingDirection+.��JHНаправление подготовки (Специальность)�� $	 280�� �� *PreparationTypeReport+.��HFТип подготовки (обучение или практика)�� $	 100�� �� *PreparationPeriod+.��Период