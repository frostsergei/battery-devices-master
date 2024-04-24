const { Builder, By } = require('selenium-webdriver');
const chrome = require('selenium-webdriver/chrome');

async function formInputTest() {
  let options = new chrome.Options();
  options.addArguments('--ignore-certificate-errors');
  let driver = await new Builder()
    .forBrowser('chrome')
    .setChromeOptions(options)
    .build();

  await driver.get('https://localhost:4200/');

  let button = await driver.findElement(
    By.xpath("//button[@mat-icon-button and @color='secondary']"),
  );
  await button.click();
  await driver.sleep(500);
  try {
    let hint = await driver.findElement(
      By.xpath("//mat-hint[text()='Подсказка для РКЧ']"),
    );
    console.log('Подсказка отображается:', await hint.getText());
  } catch (error) {
    console.log('Подсказка не отображается');
  }
  await button.click();
  await driver.sleep(500);
  try {
    let hint = await driver.findElement(By.xpath("//mat-hint[@align='start']"));
    console.log(
      'Подсказка отображается после повторного нажатия на кнопку:',
      await hint.getText(),
    );
  } catch (error) {
    console.log('Подсказка исчезла после повторного нажатия на кнопку');
  }

  let inputElement1 = await driver.findElement(By.id('mat-input-0'));
  await inputElement1.sendKeys(12345);
  await driver.sleep(100);

  let inputElement2 = await driver.findElement(By.id('mat-input-1'));
  await inputElement2.sendKeys(12345);
  await driver.sleep(100);

  let inputElement3 = await driver.findElement(By.id('mat-input-2'));
  await inputElement3.sendKeys(12345);
  await driver.sleep(100);

  let inputElement4 = await driver.findElement(By.id('mat-input-3'));
  await inputElement4.sendKeys('01-01-2000');
  await driver.sleep(500);

  let confirmButton = await driver.findElement(
    By.xpath('//button[text()="Подтвердить"]'),
  );
  await driver.sleep(3000);
  await confirmButton.click();
}

formInputTest();
