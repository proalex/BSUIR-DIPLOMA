   M S B A M L   `   `   `  ����A  =Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ClientClient  5   	   | xSystem.Windows.Controls.DataVisualization.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35�{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting 5   	       Client.MonitoringWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:Client     5   	   �chartingToolkit{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit     5   	   -   5        $��349.334��5      $��863.333��6+     � 
ResizeMode$  NoResize=�6;    � Title$H BОтслеживание процесса тестирования��6Q   .��5	      � .+�5
      >  8System.Windows.Controls.DataVisualization.Charting.Chart  -   5          Name$ 	lineChart��5
         Title$6 0График генерации нагрузки��6.   $	��Top=�6P   $��10,10,297,0q�6h   $	��311��6}      Series. 5      C  =System.Windows.Controls.DataVisualization.Charting.LineSeries  H  BSystem.Windows.Controls.DataVisualization.Charting.DataPointSeries   DependentValuePath$ Value��6*      IndependentValuePath$	 Key��6E      IsSelectionEnabled .    ItemsSource 6`   �� 	 � DataContext	 6x   �� 6�   5      �� 5      -   5        
 ��Name$
 label��5   
   $1��+Параметры тестирования��6   $
��Left=�6:   $��
646,-5,0,0q�6U   $	��Top=�6i   $��23��6�   $	��139��6�   �� 5      -   5        $
 label1��5   
   $L��FКоличество виртуальных пользователей��6   $
��Left=�6I   $��
565,23,0,0q�6d   $	��Top=�6x   $��23��6�   $	��220��6�   �� 5      -   5         ��Name$ virtualUsersControl��5      $
��Left=�6)   $��23��6D   $��
565,46,0,0q�6P    �� TextWrapping$
 Wrap=�6d   $	��Top=�6x   $	��240��6�   {�.  6�   �� 5      -   5        $
 label2��5   
   $b��\Максимальное время ожидания ответа от сервера (мс)��6   $
��Left=�6W   $��
565,74,0,0q�6r   $	��Top=�6�   �� 5      -   5        $ timeoutControl��5      $
��Left=�6$   $��23��6?   $��565,102,0,0q�6K   $
 Wrap=�6`   $	��Top=�6t   $	��240��6�   {�.  6�   �� 5      -   5        $
 label3��5   
   $W��QМинимальное время между двумя запросами (мс)��6   $
��Left=�6Q   $��565,130,0,0q�6l   $	��Top=�6�   �� 5      -	   5        $ betweenControl��5      $
��Left=�6$   $��23��6?   $��565,153,0,0q�6K   $
 Wrap=�6`   $	��Top=�6t   $	��240��6�   {�.  6�   �� 5      -
   5        $
 label4��5   
   $@��:Длительность тестирования (мин)��6   $
��Left=�6D   $��565,176,0,0q�6_   $	��Top=�6t   �� 5      -   5        $ durationControl��5      $
��Left=�6%   $��23��6@   $��565,199,0,0q�6L   $
 Wrap=�6a   $	��Top=�6u   $	��240��6�   {�.  6�   �� 5      -   5        $
 label5��5   
   $1��+Стратегия тестирования��6   $
��Left=�6;   $��565,222,0,0q�6V   $	��Top=�6k   �� 5      -   5        $ strategyControl��5      $
��Left=�6%   $��23��6@   $��565,245,0,0q�6L   $
 Wrap=�6a   $	��Top=�6u   $	��240��6�   {�.  6�   �� 5      -   5        $
 label6��5   
   $��Статус:��6   $
��Left=�6,   $��565,268,0,0q�6G   $	��Top=�6\   �� 5      -   5        $
 statusLabel��5   
   $2��,Тестирование завершено.��6   $
��Left=�6A   $��565,291,0,0q�6\   $	��Top=�6q   �� 5      -   5        $
 label7��5   
   $L��FКоличество виртуальных пользователей��6   $
��Left=�6I   $��128,292,0,0q�6d   $	��Top=�6y   $	��217��6�   $��22��6�   �� 5      -   5        $
 label8��5   
   $I��CВремя ожидания ответа от сервера (мс)��6   $
��Left=�6J   $��-93,172,0,0q�6e   $	��Top=�6z    J� RenderTransformOrigin$ 0.5,0.5&�6�   z�5      V� 5      .~�5      �� �� 5      �� 5        �� Angle$	 -90Q�6   T� 5!      5"      5$      5&      5'      