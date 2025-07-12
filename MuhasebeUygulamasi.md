# Ön Muhasebe Uygulaması - Veritabanı Tasarımı ve İşlem Adımları

## 1. Veritabanı Tabloları

### 1.1 Temel Tablolar

#### **ChartOfAccounts (Hesap Planı)**
- `Id` (Guid, PK)
- `TenantId` (Guid, FK)
- `AccountCode` (string) - Hesap kodu
- `AccountName` (string) - Hesap adı
- `AccountType` (enum) - Hesap türü (Aktif, Pasif, Gelir, Gider)
- `ParentAccountId` (Guid?, FK) - Üst hesap
- `IsActive` (bool)
- `CreatedDate` (DateTime)
- `UpdatedDate` (DateTime)
- `IsDeleted` (bool)

#### **FiscalPeriods (Mali Dönemler)**
- `Id` (Guid, PK)
- `TenantId` (Guid, FK)
- `PeriodName` (string) - Dönem adı
- `StartDate` (DateTime) - Başlangıç tarihi
- `EndDate` (DateTime) - Bitiş tarihi
- `IsActive` (bool)
- `IsClosed` (bool) - Dönem kapatıldı mı?
- `CreatedDate` (DateTime)
- `UpdatedDate` (DateTime)
- `IsDeleted` (bool)

#### **JournalEntries (Yevmiye Defteri)**
- `Id` (Guid, PK)
- `TenantId` (Guid, FK)
- `EntryNumber` (string) - Yevmiye numarası
- `EntryDate` (DateTime) - İşlem tarihi
- `Description` (string) - Açıklama
- `ReferenceNumber` (string) - Referans numarası
- `TotalDebit` (decimal) - Toplam borç
- `TotalCredit` (decimal) - Toplam alacak
- `Status` (enum) - Durum (Draft, Posted, Cancelled)
- `PostedDate` (DateTime?) - Muhasebe kaydı tarihi
- `CreatedBy` (Guid, FK)
- `CreatedDate` (DateTime)
- `UpdatedDate` (DateTime)
- `IsDeleted` (bool)

#### **JournalEntryLines (Yevmiye Satırları)**
- `Id` (Guid, PK)
- `JournalEntryId` (Guid, FK)
- `AccountId` (Guid, FK)
- `Description` (string) - Satır açıklaması
- `DebitAmount` (decimal) - Borç tutarı
- `CreditAmount` (decimal) - Alacak tutarı
- `LineNumber` (int) - Satır numarası
- `CreatedDate` (DateTime)
- `UpdatedDate` (DateTime)
- `IsDeleted` (bool)

### 1.2 Müşteri ve Tedarikçi Tabloları

#### **Customers (Müşteriler)**
- `Id` (Guid, PK)
- `TenantId` (Guid, FK)
- `CustomerCode` (string) - Müşteri kodu
- `CustomerName` (string) - Müşteri adı
- `TaxNumber` (string) - Vergi numarası
- `TaxOffice` (string) - Vergi dairesi
- `Address` (string) - Adres
- `Phone` (string) - Telefon
- `Email` (string) - E-posta
- `ContactPerson` (string) - İletişim kişisi
- `CreditLimit` (decimal) - Kredi limiti
- `PaymentTerm` (int) - Ödeme vadesi (gün)
- `IsActive` (bool)
- `CreatedDate` (DateTime)
- `UpdatedDate` (DateTime)
- `IsDeleted` (bool)

#### **Suppliers (Tedarikçiler)**
- `Id` (Guid, PK)
- `TenantId` (Guid, FK)
- `SupplierCode` (string) - Tedarikçi kodu
- `SupplierName` (string) - Tedarikçi adı
- `TaxNumber` (string) - Vergi numarası
- `TaxOffice` (string) - Vergi dairesi
- `Address` (string) - Adres
- `Phone` (string) - Telefon
- `Email` (string) - E-posta
- `ContactPerson` (string) - İletişim kişisi
- `PaymentTerm` (int) - Ödeme vadesi (gün)
- `IsActive` (bool)
- `CreatedDate` (DateTime)
- `UpdatedDate` (DateTime)
- `IsDeleted` (bool)

### 1.3 Fatura ve Ödeme Tabloları

#### **Invoices (Faturalar)**
- `Id` (Guid, PK)
- `TenantId` (Guid, FK)
- `InvoiceNumber` (string) - Fatura numarası
- `InvoiceDate` (DateTime) - Fatura tarihi
- `DueDate` (DateTime) - Vade tarihi
- `CustomerId` (Guid, FK) - Müşteri
- `SupplierId` (Guid, FK) - Tedarikçi
- `InvoiceType` (enum) - Fatura türü (Sales, Purchase)
- `SubTotal` (decimal) - Ara toplam
- `TaxAmount` (decimal) - Vergi tutarı
- `DiscountAmount` (decimal) - İndirim tutarı
- `TotalAmount` (decimal) - Genel toplam
- `CurrencyId` (Guid, FK)
- `ExchangeRate` (decimal) - Döviz kuru
- `Status` (enum) - Durum (Draft, Posted, Paid, Cancelled)
- `Notes` (string) - Notlar
- `CreatedBy` (Guid, FK)
- `CreatedDate` (DateTime)
- `UpdatedDate` (DateTime)
- `IsDeleted` (bool)

#### **InvoiceLines (Fatura Satırları)**
- `Id` (Guid, PK)
- `InvoiceId` (Guid, FK)
- `ItemDescription` (string) - Mal/hizmet açıklaması
- `Quantity` (decimal) - Miktar
- `UnitPrice` (decimal) - Birim fiyat
- `TaxRate` (decimal) - Vergi oranı
- `TaxAmount` (decimal) - Vergi tutarı
- `DiscountRate` (decimal) - İndirim oranı
- `DiscountAmount` (decimal) - İndirim tutarı
- `LineTotal` (decimal) - Satır toplamı
- `LineNumber` (int) - Satır numarası
- `CreatedDate` (DateTime)
- `UpdatedDate` (DateTime)
- `IsDeleted` (bool)

#### **Payments (Ödemeler)**
- `Id` (Guid, PK)
- `TenantId` (Guid, FK)
- `PaymentNumber` (string) - Ödeme numarası
- `PaymentDate` (DateTime) - Ödeme tarihi
- `CustomerId` (Guid, FK) - Müşteri
- `SupplierId` (Guid, FK) - Tedarikçi
- `PaymentType` (enum) - Ödeme türü (Cash, Bank, Check, CreditCard)
- `Amount` (decimal) - Tutar
- `CurrencyId` (Guid, FK)
- `ExchangeRate` (decimal) - Döviz kuru
- `ReferenceNumber` (string) - Referans numarası
- `Description` (string) - Açıklama
- `Status` (enum) - Durum (Draft, Posted, Cancelled)
- `CreatedBy` (Guid, FK)
- `CreatedDate` (DateTime)
- `UpdatedDate` (DateTime)
- `IsDeleted` (bool)

### 1.4 Banka ve Kasa Tabloları

#### **Banks (Bankalar)**
- `Id` (Guid, PK)
- `TenantId` (Guid, FK)
- `BankCode` (string) - Banka kodu
- `BankName` (string) - Banka adı
- `BranchName` (string) - Şube adı
- `AccountNumber` (string) - Hesap numarası
- `IBAN` (string) - IBAN
- `CurrencyId` (Guid, FK)
- `IsActive` (bool)
- `CreatedDate` (DateTime)
- `UpdatedDate` (DateTime)
- `IsDeleted` (bool)

#### **CashRegisters (Kasalar)**
- `Id` (Guid, PK)
- `TenantId` (Guid, FK)
- `CashRegisterCode` (string) - Kasa kodu
- `CashRegisterName` (string) - Kasa adı
- `CurrencyId` (Guid, FK)
- `IsActive` (bool)
- `CreatedDate` (DateTime)
- `UpdatedDate` (DateTime)
- `IsDeleted` (bool)

### 1.5 Raporlama Tabloları

#### **AccountBalances (Hesap Bakiyeleri)**
- `Id` (Guid, PK)
- `TenantId` (Guid, FK)
- `AccountId` (Guid, FK)
- `FiscalPeriodId` (Guid, FK)
- `OpeningDebit` (decimal) - Açılış borç
- `OpeningCredit` (decimal) - Açılış alacak
- `PeriodDebit` (decimal) - Dönem borç
- `PeriodCredit` (decimal) - Dönem alacak
- `ClosingDebit` (decimal) - Kapanış borç
- `ClosingCredit` (decimal) - Kapanış alacak
- `CreatedDate` (DateTime)
- `UpdatedDate` (DateTime)
- `IsDeleted` (bool)

## 2. İşlem Adımları

### 2.1 Hesap Planı Yönetimi

#### **Hesap Ekleme**
1. Hesap kodu ve adı girilir
2. Hesap türü seçilir (Aktif, Pasif, Gelir, Gider)
3. Üst hesap seçilir (opsiyonel)
4. Hesap aktif/pasif durumu belirlenir
5. Hesap kaydedilir

#### **Hesap Düzenleme**
1. Mevcut hesap seçilir
2. Hesap bilgileri güncellenir
3. Değişiklikler kaydedilir

#### **Hesap Silme**
1. Hesap seçilir
2. Hesabın alt hesapları kontrol edilir
3. Hesabın işlem geçmişi kontrol edilir
4. Soft delete işlemi yapılır

### 2.2 Mali Dönem Yönetimi

#### **Dönem Açma**
1. Dönem adı girilir
2. Başlangıç ve bitiş tarihleri belirlenir
3. Dönem aktif edilir
4. Önceki dönem kapatılır

#### **Dönem Kapatma**
1. Dönem seçilir
2. Dönem içi işlemler kontrol edilir
3. Hesap bakiyeleri hesaplanır
4. Dönem kapatılır

### 2.3 Yevmiye Defteri İşlemleri

#### **Yevmiye Kaydı Oluşturma**
1. Yevmiye numarası otomatik oluşturulur
2. İşlem tarihi girilir
3. Açıklama yazılır
4. Hesap satırları eklenir:
   - Hesap seçilir
   - Borç/alacak tutarı girilir
   - Satır açıklaması yazılır
5. Borç/alacak dengesi kontrol edilir
6. Kayıt taslak olarak kaydedilir

#### **Yevmiye Kaydı Muhasebe Kaydı**
1. Taslak kayıt seçilir
2. Kayıt detayları kontrol edilir
3. Muhasebe kaydı yapılır
4. Hesap bakiyeleri güncellenir
5. Kayıt durumu "Posted" olarak değiştirilir

### 2.4 Müşteri/Tedarikçi Yönetimi

#### **Müşteri Ekleme**
1. Müşteri kodu ve adı girilir
2. Vergi bilgileri girilir
3. İletişim bilgileri girilir
4. Kredi limiti ve ödeme vadesi belirlenir
5. Müşteri kaydedilir

#### **Tedarikçi Ekleme**
1. Tedarikçi kodu ve adı girilir
2. Vergi bilgileri girilir
3. İletişim bilgileri girilir
4. Ödeme vadesi belirlenir
5. Tedarikçi kaydedilir

### 2.5 Fatura İşlemleri

#### **Satış Faturası Oluşturma**
1. Müşteri seçilir
2. Fatura tarihi ve vadesi belirlenir
3. Fatura satırları eklenir:
   - Mal/hizmet açıklaması
   - Miktar ve birim fiyat
   - Vergi oranı
   - İndirim oranı
4. Fatura toplamları hesaplanır
5. Fatura taslak olarak kaydedilir

#### **Alış Faturası Oluşturma**
1. Tedarikçi seçilir
2. Fatura tarihi ve vadesi belirlenir
3. Fatura satırları eklenir
4. Fatura toplamları hesaplanır
5. Fatura taslak olarak kaydedilir

#### **Fatura Muhasebe Kaydı**
1. Fatura seçilir
2. Fatura detayları kontrol edilir
3. Otomatik yevmiye kaydı oluşturulur
4. Fatura durumu "Posted" olarak değiştirilir

### 2.6 Ödeme İşlemleri

#### **Müşteri Ödemesi**
1. Müşteri seçilir
2. Ödeme tarihi belirlenir
3. Ödeme türü seçilir (Nakit, Banka, Çek, Kredi Kartı)
4. Tutar girilir
5. Referans numarası girilir
6. Ödeme taslak olarak kaydedilir

#### **Tedarikçi Ödemesi**
1. Tedarikçi seçilir
2. Ödeme tarihi belirlenir
3. Ödeme türü seçilir
4. Tutar girilir
5. Referans numarası girilir
6. Ödeme taslak olarak kaydedilir

#### **Ödeme Muhasebe Kaydı**
1. Ödeme seçilir
2. Ödeme detayları kontrol edilir
3. Otomatik yevmiye kaydı oluşturulur
4. Ödeme durumu "Posted" olarak değiştirilir

### 2.7 Banka/Kasa İşlemleri

#### **Banka Hesabı Ekleme**
1. Banka kodu ve adı girilir
2. Şube bilgileri girilir
3. Hesap numarası ve IBAN girilir
4. Para birimi seçilir
5. Banka hesabı kaydedilir

#### **Kasa Ekleme**
1. Kasa kodu ve adı girilir
2. Para birimi seçilir
3. Kasa kaydedilir

### 2.8 Raporlama

#### **Büyük Defter Raporu**
1. Hesap seçilir
2. Dönem aralığı belirlenir
3. Açılış bakiyesi hesaplanır
4. Dönem içi işlemler listelenir
5. Kapanış bakiyesi hesaplanır
6. Rapor oluşturulur

#### **Yevmiye Defteri Raporu**
1. Tarih aralığı belirlenir
2. Yevmiye kayıtları listelenir
3. Toplam borç/alacak hesaplanır
4. Rapor oluşturulur

#### **Müşteri Hesap Ekstresi**
1. Müşteri seçilir
2. Tarih aralığı belirlenir
3. Fatura ve ödeme işlemleri listelenir
4. Bakiye hesaplanır
5. Rapor oluşturulur

#### **Tedarikçi Hesap Ekstresi**
1. Tedarikçi seçilir
2. Tarih aralığı belirlenir
3. Fatura ve ödeme işlemleri listelenir
4. Bakiye hesaplanır
5. Rapor oluşturulur

#### **Gelir Tablosu**
1. Dönem seçilir
2. Gelir hesapları listelenir
3. Dönem toplamları hesaplanır
4. Rapor oluşturulur

#### **Gider Tablosu**
1. Dönem seçilir
2. Gider hesapları listelenir
3. Dönem toplamları hesaplanır
4. Rapor oluşturulur

## 3. Güvenlik ve Yetkilendirme

### 3.1 Kullanıcı Rolleri
- **Muhasebe Müdürü**: Tüm işlemleri yapabilir
- **Muhasebe Uzmanı**: Günlük işlemleri yapabilir, raporları görüntüleyebilir
- **Muhasebe Asistanı**: Sadece giriş işlemleri yapabilir
- **Rapor Kullanıcısı**: Sadece raporları görüntüleyebilir

### 3.2 İşlem Güvenliği
- Tüm işlemler loglanır
- Kritik işlemler için onay mekanizması
- Muhasebe kaydı yapılan işlemler geri alınamaz
- Dönem kapatıldıktan sonra işlem yapılamaz

## 4. Entegrasyon Noktaları

### 4.1 Diğer Modüller ile Entegrasyon
- **Users Modülü**: Kullanıcı ve rol yönetimi
- **Customers Modülü**: Müşteri bilgileri
- **Catalog Modülü**: Ürün/hizmet bilgileri
- **Orders Modülü**: Sipariş bilgileri

### 4.2 Dış Sistem Entegrasyonları
- E-Fatura entegrasyonu
- Banka entegrasyonu
- E-Arşiv entegrasyonu
- Muhasebe paketi entegrasyonu

## 5. Teknik Uygulama Notları

### 5.1 Modül Yapısı
Bu tasarım, mevcut modüler monolith mimarisi kullanılarak `Modules/Accounting/Accounting/` altında implement edilecektir.

### 5.2 CQRS Pattern
Tüm işlemler CQRS pattern'i kullanılarak Command ve Query'ler şeklinde organize edilecektir.

### 5.3 Entity Framework Core
- Her entity için Configuration sınıfları oluşturulacak
- Migration'lar otomatik olarak oluşturulacak
- Soft delete pattern'i kullanılacak

### 5.4 Validation
- FluentValidation kullanılarak tüm Command'ler validate edilecek
- Business rule'lar Domain layer'da implement edilecek

### 5.5 Caching
- Sık kullanılan veriler (hesap planı, müşteriler, tedarikçiler) cache'lenecek
- Cache invalidation stratejisi belirlenecek

Bu tasarım, modüler monolith mimarisi kullanılarak geliştirilecek ön muhasebe uygulaması için temel bir çerçeve sunmaktadır. Her tablo ve işlem adımı, mevcut proje yapısına uygun şekilde CQRS pattern'i kullanılarak implement edilebilir.