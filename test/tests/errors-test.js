const { expect } = require('chai');
const { CODES, CLASSES } = require('../../lib/errors');


const ERROR_DATA = {
    [CODES.E000]: {
        foo: 'bar'
    },

    [CODES.E001]: void 0,

    [CODES.E002]: {
        path: '/usr/bin/browser'
    },

    [CODES.E003]: {
        binary:   '/usr/bin/browser',
        exitCode: 1,
        output:   'Segmentation fault'
    },

    [CODES.E004]: {
        binary: '/usr/bin/browser'
    },

    [CODES.E005]: {
        binary: '/usr/bin/browser'
    },

    [CODES.E006]: {
        binary: '/usr/bin/browser'
    }
};

const ERROR_MESSAGES = {
    [CODES.E000]: 'BasicError: An error was thrown.\n' +
                  'Error data:\n' +
                  '{ foo: \'bar\' }',

    [CODES.E001]: 'BrowserPathNotSetError: Unable to run the browser. The browser path or command template is not specified.',

    [CODES.E002]: 'UnableToRunBrowsersError: Unable to run the browser. The file at /usr/bin/browser does not exist or is not executable.',

    [CODES.E003]: 'NativeBinaryHasFailedError: The /usr/bin/browser process failed with the 1 exit code.\n' +
                  'Process output:\n' +
                  'Segmentation fault',

    [CODES.E004]: 'UnableToAccessAutomationAPIError: The /usr/bin/browser process cannot access the Automation API.',

    [CODES.E005]: 'UnableToAccessScreenRecordingAPIError: The /usr/bin/browser process cannot access the Screen Recording API.',

    [CODES.E006]: 'UnableToOpenDisplayError: The /usr/bin/browser process cannot open the display.'
};

describe('Errors', () => {
    it('Should render all errors', () => {
        for (const code of Object.values(CODES))
            expect(String(new CLASSES[code](ERROR_DATA[code]))).equal(ERROR_MESSAGES[code]);
    });
});
