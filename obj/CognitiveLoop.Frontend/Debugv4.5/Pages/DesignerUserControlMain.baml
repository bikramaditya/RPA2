   M S B A M L   `   `   `  ����Q  MCognitiveLoop.Frontend, Version=6.1.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:FluentTest.PagesFluentTest.Pages  5      O,clr-namespace:FluentTest.ViewModels.EntitiesFluentTest.ViewModels.Entities  5      .    (FluentTest.Pages.DesignerUserControlMain   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:FluentTest.Pages     5      ?models,clr-namespace:FluentTest.ViewModels.Entities     5      A =Fluent, Version=6.1.0.0, Culture=neutral, PublicKeyToken=nullFluenturn:fluent-ribbon  5        � DataContext  5
      ��  � 	Resources 5      2   ,FluentTest.ViewModels.Entities.ROBOConverter   ROBOConverter��%   &	          5   
   6   5      ��5      �� 5   
    �� Color$ White��6   .��5      �  � ColumnDefinitions 5   
   �� 5      $��1* �6   �� 5      $��2* �6   �� 5      $��1* �6    � RowDefinitions 5   
   �� 5      6   .+�5   
   �� $��Automation Suits��6   $��2,2,2,2q�6-   
�������
���   �.��5      R� -   5        $��TestCaseTreeView��5      $��0��5      $��0��6    $��1,5,0,0q�5      
������ 
������ ��5      ��  �� Path$&  RoboViewModel.TestSuitCollection���5"      �� 5#       �� 
TargetType" Q�6   .�5$      ��  �� Property �@Q�
IsSelected �� Value 67   �� *
IsSelected+	 �� Mode$	 TwoWay=��� 5%       �@Q�
IsExpanded 67   �� *
IsExpanded+$	 TwoWay=�5&      ��5(      �� 5)      
 �� ItemsSource
 6/   �� *AllCollection+.R�5*      ��  �� Orientation$ 
Horizontal=�6%    � ContextMenu 5+      �� 5,   "   .@�5-   &   m� -   5        $��Delete��5-   /    m� Icon 5.   *   �� 5/   .   $��/Images/delete.png��64   6S   51   '   52   #   .+�54      �� $��24��6$   $��24��6/   $��/Icons/suit.png��6;   $
��Left=�6T   $��Center=�6p   �� 55      $��10,0,0,0q�6(   $
��13.5(�6:   $
��Left=�6m   $��Center=�6�   ��6J   �� *Name+$	 TwoWay=�6�   56       �� ItemTemplate 58      �� 59      
 67   �� *TestStepCollection+.R�5:   "   �� $ 
Horizontal=�6-    5;   &   �� 5<   *   .@�5=   .   m� -   5        $��Delete��5=   7    5>   2   �� 5?   6   $��/Images/delete.png��6<   6[   5A   /   5B   +   .+�5D   &   �� $��24��6,   $��24��67   $
��Left=�6_   $��Center=�6{   ��6C   �� *
IconPath+6{   �� 5E   &   $��10,0,0,0q�60   $��13(�6B   $
��Left=�6s   $��Center=�6�   ��6P   �� *Name+$	 TwoWay=�6�   5F   #    5H   "   �� 5I   &   .R�5J   *   �� $ 
Horizontal=�65    5K   .   �� 5L   2   .@�5M   6   m� -   5        $��Delete��5M   ?    5N   :   �� 5O   >   $��/Images/delete.png��6D   6c   5Q   7   5R   3   .+�5T   .   �� $��24��64   $��24��6?   $��/Icons/step.png��6K   $
��Left=�6d   $��Center=�6�   �� 5U   .   $��10,0,0,0q�68   $
��12.5(�6J   $
��Left=�6}   $��Center=�6�   ��6Z   �� *Name+$	 TwoWay=�6�   5V   +   5W   '   5Y      5[      5]      5^      �� 5_   
   $��0��6   $��1��6"   .+�5`      �� $/��)Selected element details will appear here��6   $��2,2,2,2q�6J   
�������$��5q�6s   
���   �.��5a      �� -   5        $��detail_design_panel��5a   4   $y�	Collapsed=�6   $��0��6O   $��0��6\   $��3��6l   $��0,0,0,5q�6�   .+�5b      � -   5        $��designer_main_panel_grid��5b       5c      �� 5d      $��1* �6/   6<   �� 5e      $��3* �6/   6<   �� 5f      $��1* �6/   6<   5h      � 5i      -   5        $��designer_child_panel_grid��5i      $
��0,10q�6<    5j      �� 5k      $��1* �6/   6<   �� 5l      $��3* �6/   6<   �� 5m      $��1* �6/   6<   5o      5p      5q      5r      �� 5s   
   $��0��6   $��2��6    $��Contextual Help��60   $��2,2,2,2q�6I   
�������
���   �.��5t      � -   5        $��designer_help_panel_grid��5t      $
��0,10q�63    5u      �� 5v      6)   5x      5y      5z      5{      