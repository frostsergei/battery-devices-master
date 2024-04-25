const { Builder, By } = require('selenium-webdriver');

async function runTest() {
    let driver = await new Builder().forBrowser('chrome').build();
    await driver.get("https://localhost:4200/settings");

    let divElement = await driver.findElement(By.xpath('//div[contains(@class, "app-ace-editor")]'))
    divElement.click();
    await driver.actions().sendKeys("myYamlForm").perform();
    console.log(divElement);

    let sendButton = await driver.findElement(By.css('.btn.btn-outline-light.my-3'));
    await sendButton.click();

}

runTest();
