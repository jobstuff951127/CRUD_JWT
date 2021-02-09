import axios from "axios";
import iziToast from "izitoast";
import "izitoast/dist/css/iziToast.css";

axios.interceptors.request.use((config) => {
    const token = window.localStorage.getItem('jwt');
    if (token) config.headers.Authorization = `Bearer ${token}`;
    return config;
}, error => {
    return Promise.reject(error);
})

axios.interceptors.response.use(undefined, error => {

    if (error.message === 'Network Error' && !error.response) {
        iziToast.warning({
            progressBarColor: 'black',
            titleColor: "black",
            messageColor: "white",
            title: "Error: ",
            progressBar: true,
            timeout: 12500,
            balloon: true,
            animateInside: true,
            drag: true,
            message: "Network Error!",
            backgroundColor: "#ff2f21",
        });
    }
    else {
        const { status } = error.response;

        if (status === 404) {
            //console.log("Ooops not found!")
            iziToast.warning({
                progressBarColor: 'black',
                titleColor: "black",
                messageColor: "white",
                title: "Error: ",
                progressBar: true,
                balloon: true,
                animateInside: true,
                drag: true,
                message: "Not found!",
                backgroundColor: "#ff2f21",
            });

        }
        if (status === 500) {
            iziToast.warning({
                progressBarColor: 'black',
                titleColor: "black",
                messageColor: "white",
                title: "Error: ",
                progressBar: true,
                balloon: true,
                animateInside: true,
                drag: true,
                message: "Server Error!",
                backgroundColor: "#ff2f21",
            });
        }
        if (status === 400) {
            iziToast.warning({
                progressBarColor: 'black',
                titleColor: "black",
                messageColor: "white",
                title: "Error: ",
                progressBar: true,
                balloon: true,
                animateInside: true,
                drag: true,
                message: "Bad request!",
                backgroundColor: "#ff2f21",
            });
        }
        if (status === 401) {
            iziToast.warning({
                progressBarColor: 'black',
                titleColor: "black",
                messageColor: "white",
                title: "Error: ",
                progressBar: true,
                balloon: true,
                animateInside: true,
                drag: true,
                message: "Authorization is needed",
                backgroundColor: "#ff2f21",
            });
        }
    }
})


const responseBody = (response) => response;

const requests = {
    get: (url) => axios.get(url).then(responseBody),
    post: (url, body) => axios.post(url, body).then(responseBody),
    put: (url, body) => axios.put(url, body).then(responseBody),
    del: (url) => axios.delete(url).then(responseBody)
}


const Users = {
    list: () => requests.get('/User'),
    details: (id) => requests.get(`/User/${id}`),
    create: (user) => requests.post('/User', user),
    update: (user) => requests.put(`/User/${user.id}`, user),
    delete: (id) => requests.del(`/User/${id}`),
    logIn: (user) => requests.post(`/User/login`, user)
}

export default {
    Users
}