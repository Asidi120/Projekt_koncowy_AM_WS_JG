create database ksiazki;

use ksiazki;

create table autor(
	id_autor int auto_increment not null primary key,
    imie varchar(20) default "Nieznane",
    nazwisko varchar(30) default "Nieznane",
    rok_urodzenia smallint(4) default 0,
    kraj_pochodzenia varchar(30) default "Nieznane"
);

create table wydawnictwo(
	id_wydaw int auto_increment not null primary key,
    nazwa varchar(60) not null,
    kraj_zalozenia varchar(30) default "Nieznane",
    rok_zalozenia year default 0
);

create table uzytkownicy(
	id_uzytkownik int auto_increment not null primary key,
    nick char(40) not null unique,
    haslo char(50) not null,
    root bool default 0,
    data_zalozenia date default (curdate()),
    email varchar(100) not null unique,
    plec enum("kobieta","mężczyzna","brak") default "brak",
    check (email like '%@%.%')
);

create table ksiazka(
	id_ksiazka int auto_increment not null primary key,
    tytul varchar(50) not null,
    gatunek enum("powieść obyczajowa", "romans", "kryminał", "thriller", "horror", "fantasy", "science fiction", "powieść historyczna", "literatura młodzieżowa", "literatura dziecięca", "literatura piękna"),
    opis varchar(600) default "Brak Opisu",
    jezyk_oryginalu varchar(30) not null,
    rok_wydania year default (year(curdate())),
    liczba_stron int unsigned,
	id_autor int,
    id_wydaw int,
	foreign key(id_autor) references autor(id_autor), 
    foreign key(id_wydaw) references wydawnictwo(id_wydaw)
);

create table opinia(
	id_opinia int auto_increment not null primary key,
    id_uzytkownik int not null,
    id_ksiazka int not null,
    recenzja varchar(600),
    ocena decimal(3,1),
	data_wystawienia date default (curdate()),
    check(ocena between 1 and 10),
    foreign key(id_uzytkownik) references uzytkownicy(id_uzytkownik),
    foreign key(id_ksiazka) references ksiazka(id_ksiazka),
    check(ocena>0)
);

use ksiazki;

insert into autor (imie, nazwisko, rok_urodzenia, kraj_pochodzenia) values
('George', 'Orwell', 1903, 'Wielka Brytania'),
('J.K.', 'Rowling', 1965, 'Wielka Brytania'),
('Stephen', 'King', 1947, 'USA'),
('J.R.R.', 'Tolkien', 1892, 'Wielka Brytania'),
('Agatha', 'Christie', 1890, 'Wielka Brytania'),
('Dan', 'Brown', 1964, 'USA'),
('Markus', 'Zusak', 1975, 'Australia'),
('Haruki', 'Murakami', 1949, 'Japonia'),
('Ernest', 'Hemingway', 1899, 'USA'),
('F. Scott', 'Fitzgerald', 1896, 'USA'),
('Jane', 'Austen', 1775, 'Wielka Brytania'),
('Leo', 'Tolstoy', 1828, 'Rosja'),
('Gabriel', 'Garcia Marquez', 1927, 'Kolumbia'),
('Paulo', 'Coelho', 1947, 'Brazylia'),
('Suzanne', 'Collins', 1962, 'USA');
