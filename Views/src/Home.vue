<template>
	<v-app id="inspire">
		<v-navigation-drawer
			v-model="drawer"
			:clipped="$vuetify.breakpoint.lgAndUp"
			app
		>
			<v-list rounded dense>
				<v-tooltip color="primary" right>
					<template v-slot:activator="{ on }">
						<v-list-item
							:to="{ name: 'Home' }"
							v-bind:style="{ color: activeColor }"
							v-on="on"
						>
							<v-spacer></v-spacer>
							<v-list-item-icon>
								<v-icon>home</v-icon>
							</v-list-item-icon>
							<v-spacer></v-spacer>
						</v-list-item>
					</template>
					<span>Home</span>
				</v-tooltip>
			</v-list>
			<v-list flat rounded>
				<v-list-group
					v-for="item in items"
					:key="item.action"
					v-model="item.active"
					:prepend-icon="item.action"
					no-action
				>
					<template v-slot:activator>
						<v-list-item-content>
							<v-list-item-title v-text="item.title"></v-list-item-title>
						</v-list-item-content>
					</template>

					<v-list-item
						v-for="subItem in item.items"
						:key="subItem.title"
						:to="{ name: subItem.route }"
					>
						<v-list-item-icon>
							<v-icon color="primary" v-text="subItem.action"></v-icon>
						</v-list-item-icon>
						<v-list-item-content>
							<v-list-item-title
								class="primary--text"
								v-text="subItem.title"
							></v-list-item-title>
						</v-list-item-content>
					</v-list-item>
				</v-list-group>
			</v-list>
		</v-navigation-drawer>
		<v-app-bar
			:clipped-left="$vuetify.breakpoint.lgAndUp"
			app
			color="blue darken-3"
			dark
		>
			<v-app-bar-nav-icon @click.stop="drawer = !drawer"></v-app-bar-nav-icon>
			<v-toolbar-title style="width: 300px" class="ml-0 pl-4">
				<span class="hidden-sm-and-down">TOKA</span>
			</v-toolbar-title>
			<v-spacer></v-spacer>
			<v-tooltip right color="blue darken-3">
				<template v-slot:activator="{ on }">
					<div v-on="on">
						<v-switch
							class="mt-6"
							v-model="$vuetify.theme.dark"
							v-on="on"
							color="white"
							@click="ChangeColors"
						></v-switch>
					</div>
				</template>
				<span>Dark mode</span>
			</v-tooltip>
			<v-btn icon>
				<v-icon>apps</v-icon>
			</v-btn>

			<v-btn icon color="white" @click="LogOut()">
				<v-tooltip left color="blue darken-3">
					<template v-slot:activator="{ on }">
						<v-btn v-on="on" icon>
							<v-icon>logout</v-icon>
						</v-btn>
					</template>
					<span>Log Out</span>
				</v-tooltip>
			</v-btn>
		</v-app-bar>
		<v-main>
			<v-container class="fill-height" fluid>
				<v-slide-y-transition mode="out-in">
					<Users />
				</v-slide-y-transition>
			</v-container>
		</v-main>
		<v-footer
			color="blue darken-4"
			height="auto"
			class="font-weight-medium"
			dark
		>
			<v-col class="text-center" cols="12">
				{{ new Date().getFullYear() }} —
				<strong> © TOKA</strong>
			</v-col>
		</v-footer>
	</v-app>
</template>

<script>
	import Users from "@/components/Users";

	export default {
		name: "Home",
		components: { Users },
		data: () => ({
			activeColor: "#01579B",
			flag: false,
			drawer: null,
			items: [
				{
					action: "contacts",
					title: "Costumers",
					active: false,
					items: [{ title: "Registry", action: "recent_actors", route: "Home" }],
				},
			],
		}),
		methods: {
			LogOut: function () {
				localStorage.removeItem("jwt");
				localStorage.removeItem("user");
				this.$router.push({ name: "Login" });
			},
			ChangeColors: function () {
				this.flag = !this.flag;
				if (this.flag) {
					this.activeColor = "white";
				} else {
					this.activeColor = "#01579B";
				}
			},
		},
	};
</script>
