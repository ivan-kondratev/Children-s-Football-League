using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
//test
namespace Children_sFootballLeague
{
    public partial class Form1 : Form
    {
        MySqlConnection cn = new MySqlConnection("server=localhost; user id=root; database=football_league; password=root");
        MySqlCommand cmd;
        MySqlDataAdapter adapter;
        DataTable table;
        #region Запросы
        readonly string tournamentsBySeasonQuery = "SELECT tournamentID, tournamentName FROM tournaments where year = @year",
            tournamentTableQuery = "select t.teamID," +
                " (select clubName from club_teams join clubs on club_teams.clubID = clubs.clubID where t.teamID = club_teams.teamID) as Команда," +
                " sum(win) + sum(draw) + sum(loss) as Игр," +
                " sum(win) as Победы," +
                " sum(draw) as Ничьи," +
                " sum(loss) as Поражения," +
                " sum(goals_s) as Забито, " +
                " sum(goals_m) as Пропущено, " +
                " sum(goals_s) - sum(goals_m) as `Р/М`, " +
                " sum(win) * 3 + sum(draw) as Очки " +
                " from( " +
                " select m.homeTeamID as teamID, " +
                " IF(m.goalsHomeTeam > m.goalsGuestTeam, 1, 0) as win, " +
                " IF(m.goalsHomeTeam<m.goalsGuestTeam, 1, 0) as loss, " +
                " IF(m.goalsHomeTeam = m.goalsGuestTeam, 1, 0) as draw, " +
                " m.goalsHomeTeam as goals_s, " +
                " m.goalsGuestTeam as goals_m " +
                " from matches m " +
                " where m.`date` < NOW() and m.tournamentID = @tournamentID and m.goalsHomeTeam is not null and m.goalsGuestTeam is not null" +
                " union all " +
                " select m.guestTeamID, " +
                " IF(m.goalsHomeTeam<m.goalsGuestTeam, 1, 0) as win, " +
                " IF(m.goalsHomeTeam > m.goalsGuestTeam, 1, 0) as loss, " +
                " IF(m.goalsHomeTeam = m.goalsGuestTeam, 1, 0) as draw, " +
                " m.goalsGuestTeam as goals_s, " +
                " m.goalsHomeTeam as goals_m " +
                " from matches m " +
                " where m.`date` < NOW() and m.tournamentID = @tournamentID and m.goalsHomeTeam is not null and m.goalsGuestTeam is not null" +
                " ) t " +
                " group by teamID " +
                " order by Очки desc, Победы desc, `Р/М` desc, Забито desc;",
            tournamentYearsQuery = "SELECT tournamentYear FROM tournament_years;",
            tournamentToursQuery = "select distinct tour from matches where tournamentID = @tournamentID;",
            tournamentMatchesQuery = "select (select clubName from club_teams" +
            " join clubs on club_teams.clubID = clubs.clubID where club_teams.teamID = matches.homeTeamID) as ``," +
                " goalsHomeTeam as ``," +
                " goalsGuestTeam as ``," +
                " (select clubName from club_teams join clubs on club_teams.clubID = clubs.clubID where club_teams.teamID = matches.guestTeamID) as ``," +
                " `date` as `Дата матча`" +
                " from matches  where tour = @tour and tournamentID = @tournamentID;",
            agesQuery = "select yearOfBirth from ages;",
            clubTeamsQuery = "SELECT teamID, clubName FROM club_teams join clubs on clubs.clubID = club_teams.clubID where yearOfBirth = @yearOfBirth;",
            playersQuery = "select name as Имя, surname as Фамилия, role as `Позиция` from players" +
                " where yearOfBirth = @yearOfBirth and teamID = @teamID;",
            tournamentStatusesQuery = "SELECT statusName FROM tournament_statuses;",
            addTournamentQuery = "INSERT INTO `football_league`.`tournaments` (`tournamentName`, `yearOfBirth`, `year`, `status`)" +
                " VALUES (@tournamentName, @yearOfBirth, @year, @status);",
            addClubTeamQuery = "INSERT INTO `club_teams` (clubID, `yearOfBirth`) VALUES (@clubID, @yearOfBirth);",
            rolesQuery = "SELECT role FROM roles;",
            addPlayerQuery = "INSERT INTO `players` (`name`, `surname`, `yearOfBirth`, `teamID`, `role`)" +
                " VALUES (@name, @surname, @yearOfBirth, @teamID, @role);",
            unfinishedTournamentsQuery = "select tournamentID,tournamentName from tournaments where status != 'окончен';",
            notStartedTournamentsQuery = "select tournamentID,tournamentName from tournaments where status = 'не начат';",
            home_guestTeamQuery = "select teamID, (select clubName from club_teams" +
                " join clubs on club_teams.clubID = clubs.clubID where club_teams.teamID = club_teams_tournaments.teamID) as clubName" +
                " from club_teams_tournaments where tournamentID = @tournamentID;",
            addMatchPlayedQuery = "INSERT INTO `matches`" +
                " (`tournamentID`, `homeTeamID`, `guestTeamID`, `goalsHomeTeam`, `goalsGuestTeam`, `date`, `tour`)" +
                " VALUES (@tournamentID, @homeTeamID, @guestTeamID, @goalsHomeTeam, @goalsGuestTeam, @date, @tour);",
            addMatchNotPlayedQuery = "INSERT INTO `matches`" +
                " (`tournamentID`, `homeTeamID`, `guestTeamID`, `date`, `tour`)" +
                " VALUES (@tournamentID, @homeTeamID, @guestTeamID, @date, @tour);",
            toursQueryAM = "SELECT tour FROM tours",
            yearOfBirthTournamentQuery = "select yearOfBirth from tournaments where tournamentID = @tournamentID;",
            notParticipatingClubTeamsQuery = "select club_teams.teamID, clubName, tournamentID from club_teams " +
            " left join club_teams_tournaments on club_teams_tournaments.teamID = club_teams.teamID" +
            " left join clubs on clubs.clubID = club_teams.clubID where yearOfBirth = @yearOfBirth and tournamentID is null;",
            addClubTeamToTournamentQuery = "INSERT INTO `club_teams_tournaments` (`tournamentID`, `teamID`) VALUES (@tournamentID, @teamID);",
            clubsQuery = "SELECT clubID, clubName FROM clubs;",
            addClubQuery = "INSERT INTO `clubs` (`clubName`) VALUES (@clubName);";
        #endregion
        private void addClubButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(clubNameTextBox.Text))
            {
                MessageBox.Show("Не введено название клуба!");
                return;
            }
            try
            {
                cn.Open();
                cmd = new MySqlCommand(addClubQuery, cn);
                cmd.Parameters.AddWithValue("@clubName", clubNameTextBox.Text);
                cmd.ExecuteNonQuery();
                cn.Close();

                MessageBox.Show("Клуб добавлен!");
                clubNameTextBox.Text = "";
                clubsComboBoxACT.DataSource = GetData(clubsQuery);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                cn.Close();
                return;
            }
        }

        string yearOfBirthTournament;

        private void addClubTeamToTournamentButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(clubTeamsComboBoxACTT.Text))
            {
                MessageBox.Show("Команда не выбрана!");
                return;
            }
            try
            {
                cn.Open();
                cmd = new MySqlCommand(addClubTeamToTournamentQuery, cn);
                cmd.Parameters.AddWithValue("@tournamentID", tournamentComboBoxACTT.SelectedValue);
                cmd.Parameters.AddWithValue("@teamID", clubTeamsComboBoxACTT.SelectedValue);
                cmd.ExecuteNonQuery();
                cn.Close();

                MessageBox.Show("Команда добавлена в турнир!");
                tournamentComboBoxACTT.SelectedIndex = clubTeamsComboBoxACTT.SelectedIndex = 0;
                homeTeamComboBoxAM.DataSource = GetData(home_guestTeamQuery);
                guestTeamComboBoxAM.DataSource = GetData(home_guestTeamQuery);
                yearOfBirthTournament = GetTournamentYearOfBirth();
                clubTeamsComboBoxACTT.DataSource = GetData(notParticipatingClubTeamsQuery);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                cn.Close();
                return;
            }
        }

        private void goals_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if (!(c >= 48 && c <= 57)) e.Handled = true;
        }

        private void addMatchButton_Click(object sender, EventArgs e)
        {
            if (homeTeamComboBoxAM.SelectedValue == null)
            {
                MessageBox.Show("Команда хозяев не выбрана!");
                return;
            }
            if (guestTeamComboBoxAM.SelectedValue == null)
            {
                MessageBox.Show("Команда гостей не выбрана!");
                return;
            }
            if (homeTeamComboBoxAM.SelectedValue.ToString() == guestTeamComboBoxAM.SelectedValue.ToString())
            {
                MessageBox.Show("Нельзя добавить матч с одинаковой командой хозяев и гостей!");
                return;
            }
            if (string.IsNullOrEmpty(goalsHomeTeamTextBox.Text) && !string.IsNullOrEmpty(goalsGuestTeamTextBox.Text))
            {
                MessageBox.Show("Не введены голы хозяев!");
                return;
            }
            if (string.IsNullOrEmpty(goalsGuestTeamTextBox.Text) && !string.IsNullOrEmpty(goalsHomeTeamTextBox.Text))
            {
                MessageBox.Show("Не введены голы гостей!");
                return;
            }

            try
            {
                cn.Open();
                if (string.IsNullOrEmpty(goalsGuestTeamTextBox.Text) && string.IsNullOrEmpty(goalsHomeTeamTextBox.Text))
                {
                    cmd = new MySqlCommand(addMatchNotPlayedQuery, cn);
                    cmd.Parameters.AddWithValue("@tournamentID", tournamentComboBoxAM.SelectedValue);
                    cmd.Parameters.AddWithValue("@homeTeamID", homeTeamComboBoxAM.SelectedValue);
                    cmd.Parameters.AddWithValue("@guestTeamID", guestTeamComboBoxAM.SelectedValue);
                    cmd.Parameters.AddWithValue("@date", matchDateTimePicker.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@tour", toursComboBoxAM.Text);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd = new MySqlCommand(addMatchPlayedQuery, cn);
                    cmd.Parameters.AddWithValue("@tournamentID", tournamentComboBoxAM.SelectedValue);
                    cmd.Parameters.AddWithValue("@homeTeamID", homeTeamComboBoxAM.SelectedValue);
                    cmd.Parameters.AddWithValue("@guestTeamID", guestTeamComboBoxAM.SelectedValue);
                    cmd.Parameters.AddWithValue("@goalsHomeTeam", goalsHomeTeamTextBox.Text);
                    cmd.Parameters.AddWithValue("@goalsGuestTeam", goalsGuestTeamTextBox.Text);
                    cmd.Parameters.AddWithValue("@date", matchDateTimePicker.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@tour", toursComboBoxAM.Text);
                    cmd.ExecuteNonQuery();
                }
                cn.Close();

                MessageBox.Show("Матч добавлен!");
                tournamentComboBoxAM.SelectedIndex = toursComboBoxAM.SelectedIndex = homeTeamComboBoxAM.SelectedIndex =
                    guestTeamComboBoxAM.SelectedIndex = 0;
                goalsHomeTeamTextBox.Text = guestTeamComboBoxAM.Text = "";
                matchDateTimePicker.Value = DateTime.Now;
                matchesDataGridView.DataSource = GetData(tournamentMatchesQuery);
                tournamentTableDataGridView.DataSource = GetData(tournamentTableQuery);
            }
            catch
            {
                MessageBox.Show("В текущем турнире такой матч уже есть!");
                cn.Close();
                return;
            }
        }

        private void name_surname_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if (c >= 65 && c <= 90 || c >= 97 && c <= 122 || c == 32)
                e.Handled = true;
        }

        private void addPlayerButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nameTextBox.Text))
            {
                MessageBox.Show("Не введено имя игрока!");
                return;
            }
            if (string.IsNullOrEmpty(surnameTextBox.Text))
            {
                MessageBox.Show("Не введена фамилия игрока!");
                return;
            }
            if (string.IsNullOrEmpty(clubTeamsComboBoxAP.Text))
            {
                MessageBox.Show("Команда не выбрана! По данному возрасту пока нет команд.");
                return;
            }
            try
            {
                cn.Open();
                cmd = new MySqlCommand(addPlayerQuery, cn);
                cmd.Parameters.AddWithValue("@name", nameTextBox.Text);
                cmd.Parameters.AddWithValue("@surname", surnameTextBox.Text);
                cmd.Parameters.AddWithValue("@yearOfBirth", agesComboBoxAP.Text);
                cmd.Parameters.AddWithValue("@teamID", clubTeamsComboBoxAP.SelectedValue);
                cmd.Parameters.AddWithValue("@role", rolesComboBoxAP.Text);
                cmd.ExecuteNonQuery();
                cn.Close();

                MessageBox.Show("Игрок добавлен!");
                agesComboBoxAP.SelectedIndex = clubTeamsComboBoxAP.SelectedIndex = rolesComboBoxAP.SelectedIndex = 0;
                nameTextBox.Text = surnameTextBox.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                cn.Close();
                return;
            }
        }

        private void addClubTeamButton_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Open();
                cmd = new MySqlCommand(addClubTeamQuery, cn);
                cmd.Parameters.AddWithValue("@yearOfBirth", agesComboBoxACT.Text);
                cmd.Parameters.AddWithValue("@clubID", clubsComboBoxACT.SelectedValue);
                cmd.ExecuteNonQuery();
                cn.Close();

                MessageBox.Show("Команда добавлена!");
                clubsComboBoxACT.SelectedIndex = agesComboBoxACT.SelectedIndex = 0;
                clubTeamsComboBoxACTT.DataSource = GetData(notParticipatingClubTeamsQuery);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                cn.Close();
                return;
            }
        }

        private void tournamentNameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if (c >= 65 && c <= 90 || c >= 97 && c <= 122)
                e.Handled = true;
        }

        private void addTournamentButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tournamentNameTextBox.Text))
            {
                MessageBox.Show("Не введено название турнира!");
                return;
            }

            try
            {
                cn.Open();
                cmd = new MySqlCommand(addTournamentQuery, cn);
                cmd.Parameters.AddWithValue("@tournamentName", tournamentNameTextBox.Text);
                cmd.Parameters.AddWithValue("@yearOfBirth", agesComboBoxAT.Text);
                cmd.Parameters.AddWithValue("@year", seasonComboboxAT.Text);
                cmd.Parameters.AddWithValue("@status", tournamentStatusesComboBoxAT.Text);
                cmd.ExecuteNonQuery();
                cn.Close();

                MessageBox.Show("Турнир добавлен!");

                tournamentNameTextBox.Text = "";
                agesComboBoxAT.SelectedIndex = seasonComboboxAT.SelectedIndex = tournamentStatusesComboBoxAT.SelectedIndex = 0;
                tournamentComboBoxACTT.DataSource = GetData(notStartedTournamentsQuery);
                tournamentComboBoxAM.DataSource = GetData(unfinishedTournamentsQuery);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                //Турниры
                seasonComboBox.DataSource = GetData(tournamentYearsQuery);
                seasonComboBox.DisplayMember = "tournamentYear";

                tournamentComboBox.DataSource = GetData(tournamentsBySeasonQuery);
                tournamentComboBox.DisplayMember = "tournamentName";
                tournamentComboBox.ValueMember = "tournamentID";

                tournamentTableDataGridView.DataSource = GetData(tournamentTableQuery);
                tournamentTableDataGridView.Columns[0].Visible = false;

                toursComboBox.DataSource = GetData(tournamentToursQuery);
                toursComboBox.DisplayMember = "tour";

                matchesDataGridView.DataSource = GetData(tournamentMatchesQuery);
                for (int i = 0; i < matchesDataGridView.ColumnCount - 1; i++)
                {
                    matchesDataGridView.Columns[i].HeaderText = "";
                }


                //Игроки
                agesComboBox.DataSource = GetData(agesQuery);
                agesComboBox.DisplayMember = "yearOfBirth";

                clubTeamsComboBox.DataSource = GetData(clubTeamsQuery);
                clubTeamsComboBox.DisplayMember = "clubName";
                clubTeamsComboBox.ValueMember = "teamID";

                playersDataGridView.DataSource = GetData(playersQuery);

                //Добавить турнир
                seasonComboboxAT.DataSource = GetData(tournamentYearsQuery);
                seasonComboboxAT.DisplayMember = "tournamentYear";

                agesComboBoxAT.DataSource = GetData(agesQuery);
                agesComboBoxAT.DisplayMember = "yearOfBirth";

                tournamentStatusesComboBoxAT.DataSource = GetData(tournamentStatusesQuery);
                tournamentStatusesComboBoxAT.DisplayMember = "statusName";

                //Добавить команду в клуб
                clubsComboBoxACT.DataSource = GetData(clubsQuery);
                clubsComboBoxACT.DisplayMember = "clubName";
                clubsComboBoxACT.ValueMember = "clubID";

                agesComboBoxACT.DataSource = GetData(agesQuery);
                agesComboBoxACT.DisplayMember = "yearOfBirth";

                //Добавить игрока
                agesComboBoxAP.DataSource = GetData(agesQuery);
                agesComboBoxAP.DisplayMember = "yearOfBirth";

                clubTeamsComboBoxAP.DataSource = GetData(clubTeamsQuery);
                clubTeamsComboBoxAP.DisplayMember = "clubName";
                clubTeamsComboBoxAP.ValueMember = "teamID";

                rolesComboBoxAP.DataSource = GetData(rolesQuery);
                rolesComboBoxAP.DisplayMember = "role";

                //Добавить матч
                tournamentComboBoxAM.DataSource = GetData(unfinishedTournamentsQuery);
                tournamentComboBoxAM.DisplayMember = "tournamentName";
                tournamentComboBoxAM.ValueMember = "tournamentID";

                homeTeamComboBoxAM.DataSource = GetData(home_guestTeamQuery);
                homeTeamComboBoxAM.DisplayMember = "clubName";
                homeTeamComboBoxAM.ValueMember = "teamID";

                guestTeamComboBoxAM.DataSource = GetData(home_guestTeamQuery);
                guestTeamComboBoxAM.DisplayMember = "clubName";
                guestTeamComboBoxAM.ValueMember = "teamID";

                toursComboBoxAM.DataSource = GetData(toursQueryAM);
                toursComboBoxAM.DisplayMember = "tour";

                //Добавить команду в турнир
                tournamentComboBoxACTT.DataSource = GetData(notStartedTournamentsQuery);
                tournamentComboBoxACTT.DisplayMember = "tournamentName";
                tournamentComboBoxACTT.ValueMember = "tournamentID";
                yearOfBirthTournament = GetTournamentYearOfBirth();

                clubTeamsComboBoxACTT.DataSource = GetData(notParticipatingClubTeamsQuery);
                clubTeamsComboBoxACTT.DisplayMember = "clubName";
                clubTeamsComboBoxACTT.ValueMember = "teamID";
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1042)
                {
                    MessageBox.Show("Невозможно подключиться к любому из указанных хостов. Не удалось загрузить данные.");
                    Application.Exit();
                }
            }

        }

        DataTable GetData(string query)
        {
            cmd = new MySqlCommand(query, cn);

            if (query == tournamentsBySeasonQuery)
                cmd.Parameters.AddWithValue("@year", seasonComboBox.Text);
            else if (query == tournamentTableQuery || query == tournamentToursQuery)
                cmd.Parameters.AddWithValue("@tournamentID", tournamentComboBox.SelectedValue);
            else if (query == tournamentMatchesQuery)
            {
                cmd.Parameters.AddWithValue("@tour", toursComboBox.Text);
                cmd.Parameters.AddWithValue("@tournamentID", tournamentComboBox.SelectedValue);
            }
            else if (query == clubTeamsQuery)
                cmd.Parameters.AddWithValue("@yearOfBirth", agesComboBox.Text);
            else if (query == playersQuery)
            {
                cmd.Parameters.AddWithValue("@yearOfBirth", agesComboBox.Text);
                cmd.Parameters.AddWithValue("@teamID", clubTeamsComboBox.SelectedValue);
            }
            else if (query == home_guestTeamQuery)
                cmd.Parameters.AddWithValue("@tournamentID", tournamentComboBoxAM.SelectedValue);
            else if (query == notParticipatingClubTeamsQuery)
                cmd.Parameters.AddWithValue("@yearOfBirth", yearOfBirthTournament);


            adapter = new MySqlDataAdapter(cmd);
            table = new DataTable();
            adapter.Fill(table);
            return table;
        }
        DataTable GetData(string query, object sender)
        {
            cmd = new MySqlCommand(query, cn);

            if (query == clubTeamsQuery && sender.Equals(clubTeamsComboBoxAP))
                cmd.Parameters.AddWithValue("@yearOfBirth", agesComboBoxAP.Text);

            adapter = new MySqlDataAdapter(cmd);
            table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        string GetTournamentYearOfBirth()
        {
            string result;
            try
            {
                cn.Open();
                cmd = new MySqlCommand(yearOfBirthTournamentQuery, cn);
                cmd.Parameters.AddWithValue("@tournamentID", tournamentComboBoxACTT.SelectedValue);
                result = cmd.ExecuteScalar().ToString();
                cn.Close();
                return result;
            }
            catch
            {
                cn.Close();
                return null;
            }
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender.Equals(seasonComboBox))
            {
                tournamentComboBox.DataSource = GetData(tournamentsBySeasonQuery);
                tournamentTableDataGridView.DataSource = GetData(tournamentTableQuery);
                toursComboBox.DataSource = GetData(tournamentToursQuery);
                matchesDataGridView.DataSource = GetData(tournamentMatchesQuery);
            }
            else if (sender.Equals(tournamentComboBox))
            {
                tournamentTableDataGridView.DataSource = GetData(tournamentTableQuery);
                toursComboBox.DataSource = GetData(tournamentToursQuery);
                matchesDataGridView.DataSource = GetData(tournamentMatchesQuery);
            }
            else if (sender.Equals(agesComboBox))
            {
                clubTeamsComboBox.DataSource = GetData(clubTeamsQuery);
                playersDataGridView.DataSource = GetData(playersQuery);
            }
            else if (sender.Equals(clubTeamsComboBox))
                playersDataGridView.DataSource = GetData(playersQuery);
            else if (sender.Equals(agesComboBoxAP))
                clubTeamsComboBoxAP.DataSource = GetData(clubTeamsQuery, clubTeamsComboBoxAP);
            else if (sender.Equals(tournamentComboBoxAM))
            {
                homeTeamComboBoxAM.DataSource = GetData(home_guestTeamQuery);
                guestTeamComboBoxAM.DataSource = GetData(home_guestTeamQuery);
            }
            else if (sender.Equals(toursComboBox))
                matchesDataGridView.DataSource = GetData(tournamentMatchesQuery);
            else if (sender.Equals(tournamentComboBoxACTT))
            {
                tournamentComboBoxACTT.DisplayMember = "tournamentName";
                tournamentComboBoxACTT.ValueMember = "tournamentID";
                yearOfBirthTournament = GetTournamentYearOfBirth();
                clubTeamsComboBoxACTT.DataSource = GetData(notParticipatingClubTeamsQuery);
            }


            if (tournamentTableDataGridView.RowCount == 0)
            {
                noTournamentTableLabel.Visible = true;
                tournamentTableDataGridView.Visible = false;
            }
            else
            {
                noTournamentTableLabel.Visible = false;
                tournamentTableDataGridView.Visible = true;
            }

            if (matchesDataGridView.RowCount == 0)
            {
                matchesDataGridView.Visible = false;
                noMatchesLabel.Visible = true;

            }
            else
            {
                matchesDataGridView.Visible = true;
                noMatchesLabel.Visible = false;
            }

            if (playersDataGridView.RowCount == 0)
            {
                playersDataGridView.Visible = false;
                noPlayersLabel.Visible = true;
            }
            else
            {
                playersDataGridView.Visible = true;
                noPlayersLabel.Visible = false;
            }
        }
    }
}
