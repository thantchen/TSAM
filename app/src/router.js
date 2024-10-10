/*=========================================================================================
  File Name: router.js
  Description: Routes for vue-router. Lazy loading is enabled.
  Object Strucutre:
                    path => router path
                    name => router name
                    component(lazy loading) => component to load
                    meta : {
                      rule => which user can have access (ACL)
                      breadcrumb => Add breadcrumb to specific page
                      pageTitle => Display title besides breadcrumb
                    }
  ----------------------------------------------------------------------------------------
  Item Name: Vuexy - Vuejs, HTML & Laravel Admin Dashboard Template
  Author: Pixinvent
  Author URL: http://www.themeforest.net/user/pixinvent
==========================================================================================*/


import Vue from 'vue'
import Router from 'vue-router'
//import auth from "@/auth/authService";//
//import store from "./store/store";

//import firebase from 'firebase/app'
//import 'firebase/auth'

Vue.use(Router)

const router = new Router({
    mode: 'history',
    base: process.env.BASE_URL,
    scrollBehavior () {
        return { x: 0, y: 0 }
    },
    routes: [

        {
    // =============================================================================
    // MAIN LAYOUT ROUTES
    // =============================================================================
            path: '',
            component: () => import('./layouts/main/Main.vue'),
            children: [
                 {
                    path: '/',
                    redirect: '/apps/tsa/reports/executive-summary',
                    meta: {
                        rule: 'public',
                        authRequired: false,
                    }
                },
                {
                    path: '/key-metrics',
                    name: 'key-metrics',
                    component: () => import('./views/apps/tsa/KeyMetrics.vue'),
                    meta: {
                        rule: 'primaryOwner',
                        pageTitle: 'My Key Metrics',
                        authRequired: true,
                    }
                },
                {
                    path: '/apps/tsa/views/tsa-schedule',
                    name: 'tsa-schedule-details',
                    component: () => import('./views/apps/tsa/views/TSASchedules.vue'),
                    meta: {
                        rule: 'personnel',
                        pageTitle: 'TSA Schedule',
                        authRequired: true,
                    }
                },
                {
                    path: '/apps/tsa/views/payment-approval',
                    name: 'tsa-schedule-details',
                    component: () => import('./views/apps/tsa/views/PaymentApproval.vue'),
                    meta: {
                        rule: 'tsaTeam',
                        pageTitle: 'Payment Approval',
                        authRequired: true,
                    }
                },
                {
                    path: '/apps/tsa/inputs/tsa-schedule',
                    name: 'tsa-schedule',
                    component: () => import('./views/apps/tsa/inputs/TSASchedules.vue'),
                    meta: {
                        rule: 'tsaTeam',
                        pageTitle: 'TSA Schedule',
                        authRequired: true,
                    }
                },
                {
                    path: '/apps/tsa/inputs/tsa-schedules-details',
                    name: 'tsa-schedules-details',
                    component: () => import('./views/apps/tsa/inputs/TSASchedulesDetails.vue'),
                    meta: {
                        rule: 'tsaTeam',
                        pageTitle: 'TSA Schedule Details',
                        authRequired: true,
                    }
                },
                {
                    path: '/apps/tsa/inputs/change-log',
                    name: 'change-log',
                    component: () => import('./views/apps/tsa/inputs/ChangeLog.vue'),
                    meta: {
                        rule: 'tsaTeam',
                        pageTitle: 'Change Resolution Log',
                        authRequired: true,
                    }
                },
                {
                    path: '/apps/tsa/inputs/change-log-details',
                    name: 'change-log-details',
                    component: () => import('./views/apps/tsa/inputs/ChangeLogDetails.vue'),
                    meta: {
                        rule: 'tsaTeam',
                        pageTitle: 'Change Log Details',
                        authRequired: true,
                    }
                },
                {
                    path: '/apps/tsa/inputs/invoices',
                    name: 'invoices',
                    component: () => import('./views/apps/tsa/inputs/Invoices.vue'),
                    meta: {
                        rule: 'tsaTeam',
                        pageTitle: 'Invoices',
                        authRequired: true,
                    }
                },
                {
                    path: '/apps/tsa/inputs/invoices-details',
                    name: 'invoices-details',
                    component: () => import('./views/apps/tsa/inputs/InvoicesDetails.vue'),
                    meta: {
                        rule: 'tsaTeam',
                        pageTitle: 'Invoices Details',
                        authRequired: true,
                    }
                },
                {
                    path: '/apps/tsa/inputs/dispute-log',
                    name: 'dispute-log',
                    component: () => import('./views/apps/tsa/inputs/DisputeLog.vue'),
                    meta: {
                        rule: 'tsaTeam',
                        pageTitle: 'Dispute Invoice',
                        authRequired: true,
                    }
                },
                {
                    path: '/apps/tsa/inputs/dispute-log-details',
                    name: 'dispute-log-details',
                    component: () => import('./views/apps/tsa/inputs/DisputeLogDetails.vue'),
                    meta: {
                        rule: 'tsaTeam',
                        pageTitle: 'Dispute Log Details',
                        authRequired: true,
                    }
                },
                {
                    path: '/apps/tsa/inputs/payments',
                    name: 'payments',
                    component: () => import('./views/apps/tsa/inputs/Payments.vue'),
                    meta: {
                        rule: 'tsaTeam',
                        pageTitle: 'Payments',
                        authRequired: true,
                    }
                },
                {
                    path: '/apps/tsa/inputs/payments-details',
                    name: 'payments-details',
                    component: () => import('./views/apps/tsa/inputs/PaymentsDetails.vue'),
                    meta: {
                        rule: 'tsaTeam',
                        pageTitle: 'Payments Details',
                        authRequired: true,
                    }
                },
                {
                    path: '/apps/tsa/inputs/sell-side-notification-changes',
                    name: 'sell-side-notifiction-changes',
                    component: () => import('./views/apps/tsa/inputs/SellSideNotificationChanges.vue'),
                    meta: {
                        rule: 'tsaTeam',
                        pageTitle: 'Sell Side Notification Changes',
                        authRequired: true,
                    }
                },
                {
                    path: '/apps/tsa/inputs/sell-side-notification-disputes',
                    name: 'sell-side-notification-disputes',
                    component: () => import('./views/apps/tsa/inputs/SellSideNotificationDisputes.vue'),
                    meta: {
                        rule: 'tsaTeam',
                        pageTitle: 'Sell Side Notification Disputes',
                        authRequired: true,
                    }
                },
                {
                    path: '/apps/tsa/outputs/generate-data',
                    name: 'generate-data',
                    component: () => import('./views/apps/tsa/outputs/GenerateData.vue'),
                    meta: {
                        rule: 'tsaTeam',
                        pageTitle: 'Generate Data',
                        authRequired: true,
                    }
                },
                {
                    path: '/apps/tsa/reports/insights',
                    name: 'insights',
                    component: () => import('./views/apps/tsa/reports/Insights.vue'),
                    meta: {
                        rule: 'personnel',
                        pageTitle: 'Insights',
                        authRequired: true,
                    }
                },
                {
                    path: '/apps/tsa/reports/executive-summary',
                    name: 'executive-summary',
                    component: () => import('./views/apps/tsa/reports/ExecutiveSummary.vue'),
                    meta: {
                        rule: 'primaryOwner',
                        pageTitle: 'Executive Summary',
                        authRequired: true,
                    }
                },
                {
                    path: '/apps/tsa/reports/primary-owner',
                    name: 'primary-owner',
                    component: () => import('./views/apps/tsa/reports/PrimaryOwner.vue'),
                    meta: {
                        rule: 'primaryOwner',
                        pageTitle: 'Primary Owner',
                        authRequired: true,
                    }
                },
                {
                    path: '/apps/tsa/reports/sell-side-notification',
                    name: 'sell-side-notification',
                    component: () => import('./views/apps/tsa/reports/SellSideNotification.vue'),
                    meta: {
                        rule: 'personnel',
                        pageTitle: 'Sell Side Notification',
                        authRequired: true,
                    }
                },
                {
                    path: '/apps/tsa/reports/activity-report',
                    name: 'activity-report',
                    component: () => import('./views/apps/tsa/reports/ActivityReport.vue'),
                    meta: {
                        rule: 'primaryOwner',
                        pageTitle: 'Activity Report',
                        authRequired: true,
                    }
                },
                {
                    path: '/apps/tsa/reports/tsa-life-cycle',
                    name: 'tsa-life-cycle',
                    component: () => import('./views/apps/tsa/reports/TSALifeCycle.vue'),
                    meta: {
                        rule: 'primaryOwner',
                        pageTitle: 'TSA Life Cycle',
                        authRequired: true,
                    }
                },
                {
                    path: '/apps/tsa/reports/payment-approval',
                    name: 'payment-approval',
                    component: () => import('./views/apps/tsa/reports/PaymentApproval.vue'),
                    meta: {
                        rule: 'primaryOwner',
                        pageTitle: 'Payment Approval',
                        authRequired: true,
                    }
                },
                {
                    path: '/apps/tsa/reports/adhoc',
                    name: 'adhoc-report',
                    component: () => import('./views/apps/tsa/reports/AdHoc.vue'),
                    meta: {
                        rule: 'primaryOwner',
                        pageTitle: 'Ad-Hoc',
                        authRequired: true,
                    }
                },
                {
                    path: '/apps/tsa/variable-inputs/change-log',
                    name: 'change-request',
                    component: () => import('./views/apps/tsa/variable-inputs/ChangeLog.vue'),
                    meta: {
                        rule: 'primaryOwner',
                        pageTitle: 'Change a TSA or Sub-TSA',
                        authRequired: true,
                    }
                },
                {
                    path: '/apps/tsa/variable-inputs/add-log',
                    name: 'add-request',
                    component: () => import('./views/apps/tsa/variable-inputs/AddLog.vue'),
                    meta: {
                        rule: 'primaryOwner',
                        pageTitle: 'Add New',
                        authRequired: true,
                    }
                },
                {
                    path: '/apps/tsa/variable-inputs/dispute-log',
                    name: 'dispute-request',
                    component: () => import('./views/apps/tsa/variable-inputs/DisputeLog.vue'),
                    meta: {
                        rule: 'primaryOwner',
                        pageTitle: 'Dispute Invoice',
                        authRequired: true,
                    }
                },
                {
                    path: '/apps/tsa/variable-inputs/escalation-log',
                    name: 'escalation-request',
                    component: () => import('./views/apps/tsa/variable-inputs/EscalationLog.vue'),
                    meta: {
                        rule: 'primaryOwner',
                        pageTitle: 'Performance Standard Escalation',
                        authRequired: true,
                    }
                },
                {
                    path: '/apps/tsa/settings/user-management',
                    name: 'user-management',
                    component: () => import('./views/apps/tsa/settings/UserManagement.vue'),
                    meta: {
                        rule: 'admin',
                        pageTitle: 'User Management',
                        authRequired: true,
                    }
                },

        // =============================================================================
        // Theme Routes
        // =============================================================================



        // =============================================================================
        // Application Routes
        // =============================================================================
                {
                    path: '/apps/user/user-list',
                    name: 'app-user-list',
                    component: () => import('@/views/apps/user/user-list/UserList.vue'),
                    meta: {
                        breadcrumb: [
                            { title: 'Home', url: '/' },
                            { title: 'User' },
                            { title: 'List', active: true },
                        ],
                        pageTitle: 'User List',
                        rule: 'tsaTeam'
                    },
                },
                {
                    path: '/apps/user/user-view/:userId',
                    name: 'app-user-view',
                    component: () => import('@/views/apps/user/UserView.vue'),
                    meta: {
                        breadcrumb: [
                            { title: 'Home', url: '/' },
                            { title: 'User' },
                            { title: 'View', active: true },
                        ],
                        pageTitle: 'User View',
                        rule: 'tsaTeam'
                    },
                },
                {
                    path: '/apps/user/user-edit/:userId',
                    name: 'app-user-edit',
                    component: () => import('@/views/apps/user/user-edit/UserEdit.vue'),
                    meta: {
                        breadcrumb: [
                            { title: 'Home', url: '/' },
                            { title: 'User' },
                            { title: 'Edit', active: true },
                        ],
                        pageTitle: 'User Edit',
                        rule: 'tsaTeam'
                    },
                },

        // =============================================================================
        // Pages Routes
        // =============================================================================
 /*               {
                    path: '/pages/profile',
                    name: 'page-profile',
                    component: () => import('@/views/pages/Profile.vue'),
                    meta: {
                        breadcrumb: [
                            { title: 'Home', url: '/' },
                            { title: 'Pages' },
                            { title: 'Profile', active: true },
                        ],
                        pageTitle: 'Profile',
                        rule: 'tsaTeam'
                    },
                },
                {
                    path: '/pages/user-settings',
                    name: 'page-user-settings',
                    component: () => import('@/views/pages/user-settings/UserSettings.vue'),
                    meta: {
                        breadcrumb: [
                            { title: 'Home', url: '/' },
                            { title: 'Pages' },
                            { title: 'User Settings', active: true },
                        ],
                        pageTitle: 'Settings',
                        rule: 'tsaTeam'
                    },
                },
                {
                    path: '/pages/faq',
                    name: 'page-faq',
                    component: () => import('@/views/pages/Faq.vue'),
                    meta: {
                        breadcrumb: [
                            { title: 'Home', url: '/' },
                            { title: 'Pages' },
                            { title: 'FAQ', active: true },
                        ],
                        pageTitle: 'FAQ',
                        rule: 'tsaTeam'
                    },
                },
                {
                    path: '/pages/knowledge-base',
                    name: 'page-knowledge-base',
                    component: () => import('@/views/pages/KnowledgeBase.vue'),
                    meta: {
                        breadcrumb: [
                            { title: 'Home', url: '/' },
                            { title: 'Pages' },
                            { title: 'KnowledgeBase', active: true },
                        ],
                        pageTitle: 'KnowledgeBase',
                        rule: 'tsaTeam'
                    },
                },
                {
                    path: '/pages/knowledge-base/category',
                    name: 'page-knowledge-base-category',
                    component: () => import('@/views/pages/KnowledgeBaseCategory.vue'),
                    meta: {
                        breadcrumb: [
                            { title: 'Home', url: '/' },
                            { title: 'Pages' },
                            { title: 'KnowledgeBase', url: '/pages/knowledge-base' },
                            { title: 'Category', active: true },
                        ],
                        parent: 'page-knowledge-base',
                        rule: 'tsaTeam'
                    },
                },
                {
                    path: '/pages/knowledge-base/category/question',
                    name: 'page-knowledge-base-category-question',
                    component: () => import('@/views/pages/KnowledgeBaseCategoryQuestion.vue'),
                    meta: {
                        breadcrumb: [
                            { title: 'Home', url: '/' },
                            { title: 'Pages' },
                            { title: 'KnowledgeBase', url: '/pages/knowledge-base' },
                            { title: 'Category', url: '/pages/knowledge-base/category' },
                            { title: 'Question', active: true },
                        ],
                        parent: 'page-knowledge-base',
                        rule: 'tsaTeam'
                    },
                },
                {
                    path: '/pages/search',
                    name: 'page-search',
                    component: () => import('@/views/pages/Search.vue'),
                    meta: {
                        breadcrumb: [
                            { title: 'Home', url: '/' },
                            { title: 'Pages' },
                            { title: 'Search', active: true },
                        ],
                        pageTitle: 'Search',
                        rule: 'tsaTeam'
                    },
                },
                {
                    path: '/pages/invoice',
                    name: 'page-invoice',
                    component: () => import('@/views/pages/Invoice.vue'),
                    meta: {
                        breadcrumb: [
                            { title: 'Home', url: '/' },
                            { title: 'Pages' },
                            { title: 'Invoice', active: true },
                        ],
                        pageTitle: 'Invoice',
                        rule: 'tsaTeam'
                    },
                },
*/
            ],
        },
    // =============================================================================
    // FULL PAGE LAYOUTS
    // =============================================================================
        {
            path: '',
            component: () => import('@/layouts/full-page/FullPage.vue'),
            children: [
        // =============================================================================
        // PAGES
        // =============================================================================
                {
                    path: '/pages/login',
                    name: 'page-login',
                    component: () => import('@/views/pages/login/Login.vue'),
                    meta: {
                        rule: 'public',
                        authRequired: false,
                    }
                },
                {
                    path: '/pages/register',
                    name: 'page-register',
                    component: () => import('@/views/pages/register/Register.vue'),
                    meta: {
                        rule: 'tsaTeam'
                    }
                },
                {
                    path: '/pages/forgot-password',
                    name: 'page-forgot-password',
                    component: () => import('@/views/pages/ForgotPassword.vue'),
                    meta: {
                        rule: 'tsaTeam'
                    }
                },
                {
                    path: '/pages/reset-password',
                    name: 'page-reset-password',
                    component: () => import('@/views/pages/ResetPassword.vue'),
                    meta: {
                        rule: 'tsaTeam'
                    }
                },
                {
                    path: '/pages/lock-screen',
                    name: 'page-lock-screen',
                    component: () => import('@/views/pages/LockScreen.vue'),
                    meta: {
                        rule: 'tsaTeam'
                    }
                },
                {
                    path: '/pages/comingsoon',
                    name: 'page-coming-soon',
                    component: () => import('@/views/pages/ComingSoon.vue'),
                    meta: {
                        rule: 'tsaTeam'
                    }
                },
                {
                    path: '/pages/error-404',
                    name: 'page-error-404',
                    component: () => import('@/views/pages/Error404.vue'),
                    meta: {
                        rule: 'tsaTeam'
                    }
                },
                {
                    path: '/pages/error-500',
                    name: 'page-error-500',
                    component: () => import('@/views/pages/Error500.vue'),
                    meta: {
                        rule: 'tsaTeam'
                    }
                },
                {
                    path: '/pages/not-authorized',
                    name: 'page-not-authorized',
                    component: () => import('@/views/pages/NotAuthorized.vue'),
                    meta: {
                        rule: 'public',
                        authRequired: false
                    }
                },
                {
                    path: '/pages/maintenance',
                    name: 'page-maintenance',
                    component: () => import('@/views/pages/Maintenance.vue'),
                    meta: {
                        rule: 'tsaTeam'
                    }
                },
            ]
        },
        // Redirect to 404 page, if no match found
        {
            path: '*',
            redirect: '/pages/error-404'
        }
    ],
})

router.afterEach(() => {
  // Remove initial loading
  const appLoading = document.getElementById('loading-bg')
    if (appLoading) {
        appLoading.style.display = "none";
    }
})

router.beforeEach((to, from, next) => {
    if (!(to.path == "/pages/l" && from.path === "/")) {
        var userInfo = JSON.parse(localStorage.getItem("userInfo"));
        if (!userInfo) {
            return next("/pages/login");
        }
    }

    return next();
});

export default router
