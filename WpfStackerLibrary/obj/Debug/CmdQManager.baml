   M S B A M L   `   `   `  ����L  HWpfStackerLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:WpfStackerLibraryWpfStackerLibrary  5   7   #    WpfStackerLibrary.CmdQManager   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .myclr-namespace:WpfStackerLibrary     5   7   -   5        .��5      �   � ColumnDefinitions  5	   
   �� 5
      6!   �� 5      6!   .+�5   
   � $��0��6   $	��Top=�6    � RowDefinitions 5      �� 5      $��45 �6    6.   �� 5      6"   �� 5      6"   �� 5      6"     5      �� 5      $	��100 �6#   61   �� 5      $��50 �6#   60   �� 5      $��50 �6#   60   .+�5      �� -   5        $��10,10,10,10q�5      $��0��6*   $��0��6:   .��6\   Парковка6f   �� 5      -   5        $��10,10,10,10q�5      $��0��6*   $��1��6:   .��6^   Взять из6h   �� 5      -   5        $��10,10,10,10q�5      $��0��6*   $��2��6:   .��6^   Положить в6j   �� 5       -   5        $��10,10,10,10q�5       $��0��6*   $��3��6:   .��6^   Переложить6j   �� 5!      -   5        $��TB_Take��5!   \   $��1��6   $��1��6&   $��10,10,10,10q�63   $��2��6H   6�   �� 5"      -   5        $��TB_Push��5"   \   $��1��6   $��2��6&   $��10,10,10,10q�63   $��2��6H   6�   �� 5#      -   5        $��	TB_Trans1��5#   G   $��1��6   $��3��6&   $��
10,10,5,10q�63   6Z   �� 5$      -	   5        $��	TB_Trans2��5$   G   $��2��6   $��3��6&   $��
5,10,10,10q�63   6r   5%      � 5'   
   $��1��6    5(      �� 5)      $��45 �6    6.   �� 5*      6"   .+�5,      &   System.Windows.Controls.DataGrid  -
   5        $��	DGCmdlist��5,     $��0��6   $��1��6(      SelectionMode$ Single=�67   $��10,10,10,10q�6N      HeadersVisibility$ Column=�6�      AutoGenerateColumns .     
IsReadOnly .    FrozenColumnCount$ 0��6?  ��6c   ��  �� RelativeSource �� *FindAncestor+ �� AncestorType$ my:CmdQManagerL�	 �� AncestorLevel$	 1��
 �� Path$
 CmdQueue�   Columns 5-      4  .System.Windows.Controls.DataGridTemplateColumn  5.      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ 80 6-      Header$ Команда��69      CellTemplate 5/      �� 50      .R�51   "   �� $��Cmdname��6,   ��6;   �� $
 CmdName�6[    �� Triggers 52   "   �� 53   &    �� Value$
 park��6S    �� Binding 62   �� $
 CmdName�.a�54   *   ��  �� 
TargetName$ Cmdname��61    �� Property � �� �� Value$ Парковать��6V   6j   55   '   �� 56   &   $ trans��6S    62   �� $
 CmdName�.a�57   *   �� $ Cmdname��61    � ��$ Переместить��6V   6l   58   '   �� 59   &   $
 push��6S    62   �� $
 CmdName�.a�5:   *   �� $ Cmdname��61    � ��$ Положить��6V   6i   5;   '   �� 5<   &   $
 take��6S    62   �� $
 CmdName�.a�5=   *   �� $ Cmdname��61    � ��$ 
Взять��6V   6f   5>   '   5A      5C        5D      $ 50 6-   $ Оп#1��69    5E      �� 5F      .R�5G   "   �� $��CellOp1��6,   ��6;   �� $	
 Op1�6W    5H   "   �� 5I   &   $ -1��6O    62   �� $	
 Op1�.a�5J   *   �� $ CellOp1��61    � y�$ Hidden=�6\   6m   5K   '   5N      5P        5Q      $ 50 6-   $ Оп#2��68    5R      �� 5S      .R�5T   "   �� $��CellOp2��6,   ��6;   �� $	
 Op2�6W    5U   "   �� 5V   &   $ -1��6O    62   �� $	
 Op2�.a�5W   *   �� $ CellOp2��61    � y�$ Hidden=�6\   6m   5X   '   5[      5]        5^      $ * 6-   $  ��67    5_      �� 5`      .R�5a   "   �� -   5         � ToolTip$# Удалить команду��5a   @   .��5b   &   �� $J��D/WpfStackerLibrary;component/Images/w256h2561339252558DeleteRed1.png��6,   $��15��6z   $��15��6�   6�   5c   #   5d      5f         	CellStyle 5h      �� 5i       �� 
TargetType*  $System.Windows.Controls.DataGridCell"  6    �� Triggers 5j      �� 5k       6*   �� $
 Selected� True6L   .a�5l   "   ��  � �� �#DDFFFF6?   5m      5o      5q      �� 5r       �� Orientation$ 
Horizontal=�6   $��0��62   $��0��6B   $��2��6O   $��10q�6c   .+�5s      �� -   5        $��btn_prev_cmd��5s      $��0,0,5,0q�6-   $) #Предыдущая команда��6>   .��5t      �� $2��,/WpfStackerLibrary;component/Images/prev.png��6   6T   5u      �� 5�      -   5        $��btn_next_cmd��5�   *   $��0,0,5,0q�6   $' !Следующая команда��6>   .��5�      �� -   5        $2��,/WpfStackerLibrary;component/Images/next.png��5�      6t   5�      d� 5�      -   5        $��btn_cycle_cmd��5�   0   $��0,0,5,0q�6   $ Цикл��6E    d� 	IsChecked 6o   �� $
 Cycle� �� *FindAncestor+$ my:CmdQManagerL�$	 1��.��5�      �� -   5        $8��2/WpfStackerLibrary;component/Images/order-icon.png��5�      $
��Fill=�6x    �� StretchDirection$
 Both=�6�    J� OpacityMask 6�   Z� 6�   5�      d� 5�      $��0,0,5,0q�6   $C =Удалять текущую при квитировании��61    6]   �� $
 RemoveOnKvit� �� *FindAncestor+$ my:CmdQManagerL�$	 1��.��5�      �� -   5        $T��N/WpfStackerLibrary;component/Images/115404_32715_128_delete_forbidden_icon.png��5�      $
��Fill=�6�   $
 Both=�6�    6�   Z� 6�   5�      5�      5�      5�      5�      