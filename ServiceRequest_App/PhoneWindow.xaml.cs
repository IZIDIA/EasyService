using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ServiceRequest_App
{
    public partial class PhoneWindow : Window
    {

        MainWindow MainWin;
        public PhoneWindow(MainWindow mainWin)
        {
            InitializeComponent();
            MainWin = mainWin;
            LoadPhoneListData();

        }


        //Закрытие
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWin.PhoneWindowOpened = false;
        }


        public ObservableCollection<PhoneNumber> PhoneNumbers;

        //Загрузка номеров телефонов
        public void LoadPhoneListData()
        {
            PhoneNumbers = new ObservableCollection<PhoneNumber>
            {
            new PhoneNumber {Id=1, ImagePath="source/phones/viber.png", PhoneCallNumber="+79885630977", PhoneUserName="Гуляев Платон Артемович",PhoneDescription ="Отдел технической поддержки",PhoneType="Viber" },
            new PhoneNumber {Id=2, ImagePath="source/phones/discord.png", PhoneCallNumber="work_oaovnds", PhoneUserName="Веселов Эльдар Ильяович",PhoneDescription ="Отдел системного администрирования",PhoneType="Discord" },
            new PhoneNumber {Id=3, ImagePath="source/phones/mobile.png", PhoneCallNumber="+79995895411", PhoneUserName="Назаров Мирон Даниилович",PhoneDescription ="Обслуживание ПО",PhoneType="Мобильный" },
            new PhoneNumber {Id=4, ImagePath="source/phones/work.png", PhoneCallNumber="42765", PhoneUserName="Назаров Мирон Даниилович",PhoneDescription ="Отдел технической поддержки",PhoneType="Стационарный" },
            new PhoneNumber {Id=5, ImagePath="source/phones/whatsapp.png", PhoneCallNumber="+79504789233", PhoneUserName="Сидоров Святослав Семёнович",PhoneDescription ="Отдел системного администрирования",PhoneType="WhatsApp" },
            new PhoneNumber {Id=6, ImagePath="source/phones/telegram.png", PhoneCallNumber="+79995898744", PhoneUserName="Вишняков Марк Ефимович",PhoneDescription ="Вопросы по оформлению заявок",PhoneType="Telegram" }

            };
            phonesList.Items.Clear();
            phonesList.ItemsSource = PhoneNumbers;
        }
    }
}
